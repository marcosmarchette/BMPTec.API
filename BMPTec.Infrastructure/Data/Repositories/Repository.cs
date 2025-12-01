using BMPTec.Domain.Common;
using BMPTec.Domain.Interfaces;
using BMPTec.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace BMPTec.Infrastructure.Data.Repositories
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly BmpTecContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(BmpTecContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual async Task<T?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbSet.FindAsync([id], cancellationToken);
        }

        public virtual async Task<IEnumerable<T>> ObterTodosAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet.ToListAsync(cancellationToken);
        }

        public virtual async Task AdicionarAsync(T entidade, CancellationToken cancellationToken = default)
        {
            await _dbSet.AddAsync(entidade, cancellationToken);
        }

        public virtual void Atualizar(T entidade)
        {
            _dbSet.Update(entidade);
        }

        public virtual void Remover(T entidade)
        {
            _dbSet.Remove(entidade);
        }
    }
}
