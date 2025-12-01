using BMPTec.Application.Common;
using BMPTec.Application.Common.Pagination;
using BMPTec.Application.DTOs;
using BMPTec.Domain.Enums;
using MediatR;

namespace BMPTec.Application.Features.Transferencias.Queries.ListarTRansferenciasDetalhadas
{
    public class ListarTransferenciasDetalhadasQuery : IRequest<Result<PagedResult<TransferenciaDetalhadaDto>>>
    {
        public StatusTransferencia? Status { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
