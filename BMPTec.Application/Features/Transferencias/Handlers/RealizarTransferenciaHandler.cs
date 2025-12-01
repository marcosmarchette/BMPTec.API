using AutoMapper;
using BMPTec.Application.Common;
using BMPTec.Application.DTOs;
using BMPTec.Application.Features.Transferencias.Commands.RealizarTransferencia;
using BMPTec.Domain.Entities;
using BMPTec.Domain.Enums;
using BMPTec.Domain.Interfaces;
using BMPTec.Infrastructure.Data.Services;
using MediatR;

namespace BMPTec.Application.Features.Transferencias.Handlers
{
    public class RealizarTransferenciaHandler : IRequestHandler<RealizarTransferenciaCommand, Result<TransferenciaDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly DiaUtilService _diaUtilService;

        public RealizarTransferenciaHandler(IUnitOfWork unitOfWork, IMapper mapper, DiaUtilService diaUtilService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _diaUtilService = diaUtilService;
        }

        public async Task<Result<TransferenciaDto>> Handle(RealizarTransferenciaCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Valida dia útil
                var dataAtual = DateTime.UtcNow;
                var ehDiaUtil = await _diaUtilService.EhDiaUtilAsync(dataAtual, cancellationToken);

                if (!ehDiaUtil)
                    return Result<TransferenciaDto>.Failure("Transferências só podem ser realizadas em dias úteis.");

                // Busca as contas
                var contaOrigem = await _unitOfWork.Contas.ObterPorIdAsync(request.ContaOrigemId, cancellationToken);
                if (contaOrigem == null)
                    return Result<TransferenciaDto>.Failure("Conta de origem não encontrada.");

                var contaDestino = await _unitOfWork.Contas.ObterPorIdAsync(request.ContaDestinoId, cancellationToken);
                if (contaDestino == null)
                    return Result<TransferenciaDto>.Failure("Conta de destino não encontrada.");

                // Valida contas ativas
                if (!contaOrigem.Ativa)
                    return Result<TransferenciaDto>.Failure("Conta de origem está inativa.");

                if (!contaDestino.Ativa)
                    return Result<TransferenciaDto>.Failure("Conta de destino está inativa.");

                // Valida saldo disponível
                if (contaOrigem.ObterSaldoDisponivel() < request.Valor)
                    return Result<TransferenciaDto>.Failure("Saldo insuficiente para realizar a transferência.");

                // Converte o tipo de transferência
                if (!Enum.TryParse<TipoTransferencia>(request.TipoTransferencia, out var tipoTransferencia))
                    return Result<TransferenciaDto>.Failure("Tipo de transferência inválido.");

                // Cria a transferência
                var transferencia = Transferencia.Criar(
                    request.ContaOrigemId,
                    request.ContaDestinoId,
                    request.Valor,
                    request.Descricao,
                    tipoTransferencia
                );

                await _unitOfWork.Transferencias.AdicionarAsync(transferencia, cancellationToken);

                // Realiza débito e crédito
                var saldoAnteriorOrigem = contaOrigem.Saldo;
                var saldoAnteriorDestino = contaDestino.Saldo;

                contaOrigem.Debitar(request.Valor);
                contaDestino.Creditar(request.Valor);

                _unitOfWork.Contas.Atualizar(contaOrigem);
                _unitOfWork.Contas.Atualizar(contaDestino);

                // Aprova a transferência
                transferencia.Aprovar();

                // Cria extratos
                var extratoOrigem = Extrato.Criar(
                    contaOrigem.Id,
                    TipoOperacao.TransferenciaEnviada,
                    -request.Valor,
                    saldoAnteriorOrigem,
                    contaOrigem.Saldo,
                    request.Descricao ?? "Transferência enviada",
                    transferencia.Id
                );

                var extratoDestino = Extrato.Criar(
                    contaDestino.Id,
                    TipoOperacao.TransferenciaRecebida,
                    request.Valor,
                    saldoAnteriorDestino,
                    contaDestino.Saldo,
                    request.Descricao ?? "Transferência recebida",
                    transferencia.Id
                );

                await _unitOfWork.Extratos.AdicionarAsync(extratoOrigem, cancellationToken);
                await _unitOfWork.Extratos.AdicionarAsync(extratoDestino, cancellationToken);

                await _unitOfWork.CommitAsync(cancellationToken);

                var transferenciaDto = _mapper.Map<TransferenciaDto>(transferencia);
                transferenciaDto.ContaOrigemNumero = _mapper.Map<ContaDto>(contaOrigem.NumeroConta.ToString()).NumeroConta;
                transferenciaDto.ContaDestinoNumero = _mapper.Map<ContaDto>(contaDestino.NumeroConta.ToString()).NumeroConta;

                return Result<TransferenciaDto>.Success(transferenciaDto, "Transferência realizada com sucesso.");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync(cancellationToken);
                return Result<TransferenciaDto>.Failure($"Erro ao realizar transferência: {ex.Message}");
            }
        }
    }
}
