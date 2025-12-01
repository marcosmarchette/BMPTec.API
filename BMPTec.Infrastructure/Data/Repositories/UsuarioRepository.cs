using BMPTec.Domain.Entities;
using BMPTec.Domain.Interfaces;
using BMPTec.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace BMPTec.Infrastructure.Data.Repositories
{
    public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(BmpTecContext context) : base(context)
        {
        }

        public async Task<Usuario?> ObterPorEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .FirstOrDefaultAsync(u => u.Email.Endereco == email.ToLowerInvariant(), cancellationToken);
        }

        public async Task<Usuario?> ObterPorCPFAsync(string cpf, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .FirstOrDefaultAsync(u => u.CPF.Numero == cpf, cancellationToken);
        }

        public async Task<bool> EmailExisteAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AnyAsync(u => u.Email.Endereco == email.ToLowerInvariant(), cancellationToken);
        }

        public async Task<bool> CPFExisteAsync(string cpf, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AnyAsync(u => u.CPF.Numero == cpf, cancellationToken);
        }

        public async Task<IEnumerable<Usuario>> ObterAtivosAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(u => u.Ativo)
                .ToListAsync(cancellationToken);
        }
    }
}
