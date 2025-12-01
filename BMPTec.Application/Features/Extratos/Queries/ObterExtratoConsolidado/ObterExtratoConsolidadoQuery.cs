using BMPTec.Application.Common;
using BMPTec.Application.DTOs;
using MediatR;

namespace BMPTec.Application.Features.Extratos.Queries.ObterExtratoConsolidado
{
    public class ObterExtratoConsolidadoQuery : IRequest<Result<List<ExtratoConsolidadoDto>>>
    {
        public Guid ContaId { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
    }
}