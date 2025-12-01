using AutoMapper;
using AutoMapper.QueryableExtensions;
using BMPTec.Application.Common;
using BMPTec.Application.Common.Pagination;
using BMPTec.Application.DTOs;
using BMPTec.Domain.Enums;
using BMPTec.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace BMPTec.Application.Features.Transferencias.Handlers
{
    public class ListarTransferenciasDetalhadasHandler
    {
        private readonly BmpTecContext _context;
        private readonly IMapper _mapper;

        public ListarTransferenciasDetalhadasHandler(BmpTecContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<PagedResult<TransferenciaDetalhadaDto>>> Handle(ListarTransferenciasDetalhadasRequest request)
        {
            var query = _context.Transferencias
                .Include(t => t.ContaOrigem)
                    .ThenInclude(c => c.Usuario)
                .Include(t => t.ContaDestino)
                    .ThenInclude(c => c.Usuario)
                .AsQueryable();

            // Filtros opcionais
            if (!string.IsNullOrWhiteSpace(request.Status) && Enum.TryParse<StatusTransferencia>(request.Status, out var status))
                query = query.Where(t => t.Status == status);

            if (request.DataInicio.HasValue)
                query = query.Where(t => t.DataTransferencia >= request.DataInicio);

            // Manually paginate if ToPaginatedListAsync is not available
            var projectedQuery = query
                .ProjectTo<TransferenciaDetalhadaDto>(_mapper.ConfigurationProvider);

            var totalCount = await projectedQuery.CountAsync();
            var items = await projectedQuery
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            var pagedResult = new PagedResult<TransferenciaDetalhadaDto>(
                items,
                request.PageNumber,
                request.PageSize,
                totalCount
            );

            return Result<PagedResult<TransferenciaDetalhadaDto>>.Success(pagedResult);
        }
    }
}
