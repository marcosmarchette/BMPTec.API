using AutoMapper;
using BMPTec.Application.Common;
using BMPTec.Application.DTOs;
using BMPTec.Domain.Interfaces;
using MediatR;

namespace BMPTec.Application.Features.Extratos.Queries.GerarExtrato.Handler
{
    public class GerarExtratoHandler : IRequestHandler<GerarExtratoQuery, Result<List<ExtratoDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GerarExtratoHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<List<ExtratoDto>>> Handle(GerarExtratoQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Verifica se a conta existe
                var conta = await _unitOfWork.Contas.ObterPorIdAsync(request.ContaId, cancellationToken);
                if (conta == null)
                    return Result<List<ExtratoDto>>.Failure("Conta não encontrada.");

                // Busca os extratos por período
                var extratos = await _unitOfWork.Extratos.ObterPorContaEPeriodoAsync(
                    request.ContaId,
                    request.DataInicio,
                    request.DataFim,
                    cancellationToken
                );

                var extratosDto = _mapper.Map<List<ExtratoDto>>(extratos);

                return Result<List<ExtratoDto>>.Success(extratosDto, $"Extrato gerado com sucesso. Total de {extratosDto.Count} registros.");
            }
            catch (Exception ex)
            {
                return Result<List<ExtratoDto>>.Failure($"Erro ao gerar extrato: {ex.Message}");
            }
        }
    }
}
