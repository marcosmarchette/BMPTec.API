using BMPTec.Domain.Entities;
using BMPTec.Domain.Interfaces;
using BMPTec.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace BMPTec.Infrastructure.Data.Repositories
{
    public class FeriadoRepository : Repository<Feriado>, IFeriadoRepository
    {
        public FeriadoRepository(BmpTecContext context) : base(context)
        {
        }

        public async Task<Feriado?> ObterPorDataAsync(DateTime data, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .FirstOrDefaultAsync(f => f.Data.Date == data.Date, cancellationToken);
        }

        public async Task<IEnumerable<Feriado>> ObterPorAnoAsync(int ano, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(f => f.Data.Year == ano)
                .OrderBy(f => f.Data)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> DataEhFeriadoAsync(DateTime data, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AnyAsync(f => f.Data.Date == data.Date, cancellationToken);
        }
    }
}
