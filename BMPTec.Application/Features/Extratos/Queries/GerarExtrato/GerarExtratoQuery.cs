using BMPTec.Application.Common;
using BMPTec.Application.DTOs;
using MediatR;

namespace BMPTec.Application.Features.Extratos.Queries.GerarExtrato
{
    public record GerarExtratoQuery : IRequest<Result<List<ExtratoDto>>>
    {
        public Guid ContaId { get; init; }
        public DateTime DataInicio { get; init; }
        public DateTime DataFim { get; init; }
    }
}
