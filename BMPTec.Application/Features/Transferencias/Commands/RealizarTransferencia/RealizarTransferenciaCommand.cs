using BMPTec.Application.Common;
using BMPTec.Application.DTOs;
using MediatR;

namespace BMPTec.Application.Features.Transferencias.Commands.RealizarTransferencia
{
    public record RealizarTransferenciaCommand : IRequest<Result<TransferenciaDto>>
    {
        public Guid ContaOrigemId { get; init; }
        public Guid ContaDestinoId { get; init; }
        public decimal Valor { get; init; }
        public string? Descricao { get; init; }
        public string TipoTransferencia { get; init; } = "TED";
    }
}
