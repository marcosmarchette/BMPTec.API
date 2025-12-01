using BMPTec.Domain.Entities;
using BMPTec.Domain.Interfaces;
using BMPTec.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace BMPTec.Infrastructure.Data.Repositories
{
    public class ExtratoRepository : Repository<Extrato>, IExtratoRepository
    {
        public ExtratoRepository(BmpTecContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Extrato>> ObterPorContaAsync(Guid contaId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(e => e.ContaId == contaId)
                .Include(e => e.Conta)
                .Include(e => e.Transferencia)
                .OrderByDescending(e => e.DataOperacao)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Extrato>> ObterPorContaEPeriodoAsync(Guid contaId, DateTime dataInicio, DateTime dataFim, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(e => e.ContaId == contaId &&
                            e.DataOperacao >= dataInicio &&
                            e.DataOperacao <= dataFim)
                .Include(e => e.Conta)
                .Include(e => e.Transferencia)
                .OrderByDescending(e => e.DataOperacao)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Extrato>> ObterPorTransferenciaAsync(Guid transferenciaId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(e => e.TransferenciaId == transferenciaId)
                .Include(e => e.Conta)
                .Include(e => e.Transferencia)
                .ToListAsync(cancellationToken);
        }
    }
}
