using BMPTec.Application.Common;
using BMPTec.Application.DTOs;
using BMPTec.Domain.Interfaces;
using MediatR;

namespace BMPTec.Application.Features.Transferencias.Queries.ObterTransferenciaDetalhada
{
    public class ObterTransferenciaDetalhadaHandler : IRequestHandler<ObterTransferenciaDetalhadaQuery, Result<TransferenciaDetalhadaDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ObterTransferenciaDetalhadaHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<TransferenciaDetalhadaDto>> Handle(ObterTransferenciaDetalhadaQuery request, CancellationToken cancellationToken)
        {
            var transferencia = await _unitOfWork.Transferencias.ObterPorIdAsync(request.TransferenciaId, cancellationToken);

            if (transferencia == null)
                return Result<TransferenciaDetalhadaDto>.Failure("Transferência não encontrada.");

            // Busca contas e usuários
            var contaOrigem = await _unitOfWork.Contas.ObterPorIdAsync(transferencia.ContaOrigemId, cancellationToken);
            var contaDestino = await _unitOfWork.Contas.ObterPorIdAsync(transferencia.ContaDestinoId, cancellationToken);

            if (contaOrigem == null || contaDestino == null)
                return Result<TransferenciaDetalhadaDto>.Failure("Contas não encontradas.");

            var usuarioOrigem = await _unitOfWork.Usuarios.ObterPorIdAsync(contaOrigem.UsuarioId, cancellationToken);
            var usuarioDestino = await _unitOfWork.Usuarios.ObterPorIdAsync(contaDestino.UsuarioId, cancellationToken);

            if (usuarioOrigem == null || usuarioDestino == null)
                return Result<TransferenciaDetalhadaDto>.Failure("Usuários não encontrados.");

            var transferenciaDetalhada = new TransferenciaDetalhadaDto
            {
                TransferenciaId = transferencia.Id,
                Valor = transferencia.Valor,
                Descricao = transferencia.Descricao,
                DataTransferencia = transferencia.DataTransferencia,
                DataProcessamento = transferencia.DataProcessamento,
                Status = transferencia.Status.ToString(),
                MotivoRejeicao = transferencia.MotivoRejeicao,
                TipoTransferencia = transferencia.TipoTransferencia.ToString(),
                ContaOrigemId = contaOrigem.Id,
                ContaOrigemNumero = contaOrigem.NumeroConta.Numero,
                ContaOrigemAgencia = contaOrigem.NumeroConta.Agencia,
                ContaOrigemTitular = usuarioOrigem.Nome,
                ContaDestinoId = contaDestino.Id,
                ContaDestinoNumero = contaDestino.NumeroConta.Numero,
                ContaDestinoAgencia = contaDestino.NumeroConta.Agencia,
                ContaDestinoTitular = usuarioDestino.Nome
            };

            return Result<TransferenciaDetalhadaDto>.Success(transferenciaDetalhada);
        }
    }
}