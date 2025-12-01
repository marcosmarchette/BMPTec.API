using BMPTec.Domain.Entities;
using BMPTec.Domain.Enums;
using BMPTec.Domain.Interfaces;
using BMPTec.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace BMPTec.Infrastructure.Data.Repositories
{
    public class TransferenciaRepository : Repository<Transferencia>, ITransferenciaRepository
    {
        public TransferenciaRepository(BmpTecContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Transferencia>> ObterPorContaOrigemAsync(Guid contaOrigemId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(t => t.ContaOrigemId == contaOrigemId)
                .Include(t => t.ContaOrigem)
                .Include(t => t.ContaDestino)
                .OrderByDescending(t => t.DataTransferencia)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Transferencia>> ObterPorContaDestinoAsync(Guid contaDestinoId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(t => t.ContaDestinoId == contaDestinoId)
                .Include(t => t.ContaOrigem)
                .Include(t => t.ContaDestino)
                .OrderByDescending(t => t.DataTransferencia)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Transferencia>> ObterPorStatusAsync(StatusTransferencia status, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(t => t.Status == status)
                .Include(t => t.ContaOrigem)
                .Include(t => t.ContaDestino)
                .OrderByDescending(t => t.DataTransferencia)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Transferencia>> ObterPorPeriodoAsync(DateTime dataInicio, DateTime dataFim, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(t => t.DataTransferencia >= dataInicio && t.DataTransferencia <= dataFim)
                .Include(t => t.ContaOrigem)
                .Include(t => t.ContaDestino)
                .OrderByDescending(t => t.DataTransferencia)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Transferencia>> ObterPorContaEPeriodoAsync(Guid contaId, DateTime dataInicio, DateTime dataFim, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(t => (t.ContaOrigemId == contaId || t.ContaDestinoId == contaId) &&
                            t.DataTransferencia >= dataInicio &&
                            t.DataTransferencia <= dataFim)
                .Include(t => t.ContaOrigem)
                .Include(t => t.ContaDestino)
                .OrderByDescending(t => t.DataTransferencia)
                .ToListAsync(cancellationToken);
        }
    }
}
