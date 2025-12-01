using BMPTec.Application.Common;
using MediatR;

namespace BMPTec.Application.Features.Contas.Queries.ObterContaDetalhada
{
    public class ObterContaDetalhadaQuery : IRequest<Result<ContaDetalhadaDto>>
    {
        public Guid ContaId { get; set; }
    }
}
