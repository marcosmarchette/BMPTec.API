using BMPTec.Domain.Entities;
using BMPTec.Domain.Interfaces;
using BMPTec.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace BMPTec.Infrastructure.Data.Repositories
{
    public class ContaRepository : Repository<Conta>, IContaRepository
    {
        public ContaRepository(BmpTecContext context) : base(context)
        {
        }

        public async Task<Conta?> ObterPorNumeroContaAsync(string numeroConta, int agencia, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(c => c.Usuario)
                .FirstOrDefaultAsync(c => c.NumeroConta.Numero == numeroConta && c.NumeroConta.Agencia == agencia, cancellationToken);
        }

        public async Task<IEnumerable<Conta>> ObterPorUsuarioIdAsync(Guid usuarioId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(c => c.UsuarioId == usuarioId)
                .Include(c => c.Usuario)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Conta>> ObterAtivasAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(c => c.Ativa)
                .Include(c => c.Usuario)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> NumeroContaExisteAsync(string numeroConta, int agencia, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AnyAsync(c => c.NumeroConta.Numero == numeroConta && c.NumeroConta.Agencia == agencia, cancellationToken);
        }
    }
}
