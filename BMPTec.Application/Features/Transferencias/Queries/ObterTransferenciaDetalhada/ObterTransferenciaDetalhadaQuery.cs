using BMPTec.Application.Common;
using BMPTec.Application.DTOs;
using MediatR;

namespace BMPTec.Application.Features.Transferencias.Queries.ObterTransferenciaDetalhada
{
    public class ObterTransferenciaDetalhadaQuery : IRequest<Result<TransferenciaDetalhadaDto>>
    {
        public Guid TransferenciaId { get; set; }
    }
}
