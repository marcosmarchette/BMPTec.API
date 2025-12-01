using BMPTec.Application.Common;
using BMPTec.Application.DTOs;
using BMPTec.Domain.Enums;
using BMPTec.Domain.Interfaces;
using MediatR;

namespace BMPTec.Application.Features.Extratos.Queries.ObterExtratoConsolidado
{
    public class ObterExtratoConsolidadoHandler : IRequestHandler<ObterExtratoConsolidadoQuery, Result<List<ExtratoConsolidadoDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ObterExtratoConsolidadoHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<List<ExtratoConsolidadoDto>>> Handle(ObterExtratoConsolidadoQuery request, CancellationToken cancellationToken)
        {
            // Valida se a conta existe
            var conta = await _unitOfWork.Contas.ObterPorIdAsync(request.ContaId, cancellationToken);
            if (conta == null)
                return Result<List<ExtratoConsolidadoDto>>.Failure("Conta não encontrada.");

            // Busca usuário
            var usuario = await _unitOfWork.Usuarios.ObterPorIdAsync(conta.UsuarioId, cancellationToken);
            if (usuario == null)
                return Result<List<ExtratoConsolidadoDto>>.Failure("Usuário não encontrado.");

            // Busca extratos por período ou todos
            IEnumerable<BMPTec.Domain.Entities.Extrato> extratos;

            if (request.DataInicio.HasValue && request.DataFim.HasValue)
            {
                extratos = await _unitOfWork.Extratos.ObterPorContaEPeriodoAsync(
                    request.ContaId,
                    request.DataInicio.Value,
                    request.DataFim.Value,
                    cancellationToken);
            }
            else
            {
                extratos = await _unitOfWork.Extratos.ObterPorContaAsync(request.ContaId, cancellationToken);
            }

            var extratosConsolidados = new List<ExtratoConsolidadoDto>();

            foreach (var extrato in extratos)
            {
                var extratoDto = new ExtratoConsolidadoDto
                {
                    ExtratoId = extrato.Id,
                    ContaId = conta.Id,
                    NumeroConta = conta.NumeroConta.Numero,
                    Agencia = conta.NumeroConta.Agencia,
                    Titular = usuario.Nome,
                    TipoOperacao = extrato.TipoOperacao.ToString(),
                    Valor = extrato.Valor,
                    SaldoAnterior = extrato.SaldoAnterior,
                    SaldoAtual = extrato.SaldoAtual,
                    DataOperacao = extrato.DataOperacao,
                    Descricao = extrato.Descricao,
                    NumeroDocumento = extrato.NumeroDocumento,
                    TransferenciaId = extrato.TransferenciaId
                };

                // Determina conta relacionada (similar ao CASE WHEN da view)
                if (extrato.TransferenciaId.HasValue &&
                    (extrato.TipoOperacao == TipoOperacao.TransferenciaEnviada ||
                     extrato.TipoOperacao == TipoOperacao.TransferenciaRecebida))
                {
                    var transferencia = await _unitOfWork.Transferencias.ObterPorIdAsync(extrato.TransferenciaId.Value, cancellationToken);

                    if (transferencia != null)
                    {
                        if (transferencia.ContaOrigemId == request.ContaId)
                        {
                            // É transferência enviada - mostrar conta destino
                            var contaDestino = await _unitOfWork.Contas.ObterPorIdAsync(transferencia.ContaDestinoId, cancellationToken);
                            if (contaDestino != null)
                            {
                                extratoDto.ContaRelacionada = $"Para: {contaDestino.NumeroConta.Numero}";
                            }
                        }
                        else
                        {
                            // É transferência recebida - mostrar conta origem
                            var contaOrigem = await _unitOfWork.Contas.ObterPorIdAsync(transferencia.ContaOrigemId, cancellationToken);
                            if (contaOrigem != null)
                            {
                                extratoDto.ContaRelacionada = $"De: {contaOrigem.NumeroConta.Numero}";
                            }
                        }
                    }
                }

                extratosConsolidados.Add(extratoDto);
            }

            return Result<List<ExtratoConsolidadoDto>>.Success(
                extratosConsolidados,
                $"Extrato consolidado gerado com {extratosConsolidados.Count} registros.");
        }
    }
}