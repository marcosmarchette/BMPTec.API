using BMPTec.Domain.Interfaces;
using BMPTec.Infrastructure.Data.Context;
using BMPTec.Infrastructure.Data.Repositories;

namespace BMPTec.Infrastructure.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly BmpTecContext _context;
        private IUsuarioRepository? _usuarios;
        private IContaRepository? _contas;
        private ITransferenciaRepository? _transferencias;
        private IExtratoRepository? _extratos;
        private IFeriadoRepository? _feriados;
        private bool _disposed;

        public UnitOfWork(BmpTecContext context)
        {
            _context = context;
        }

        public IUsuarioRepository Usuarios =>
            _usuarios ??= new UsuarioRepository(_context);

        public IContaRepository Contas =>
            _contas ??= new ContaRepository(_context);

        public ITransferenciaRepository Transferencias =>
            _transferencias ??= new TransferenciaRepository(_context);

        public IExtratoRepository Extratos =>
            _extratos ??= new ExtratoRepository(_context);

        public IFeriadoRepository Feriados =>
            _feriados ??= new FeriadoRepository(_context);

        public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            await Task.Run(() =>
            {
                foreach (var entry in _context.ChangeTracker.Entries())
                {
                    entry.State = Microsoft.EntityFrameworkCore.EntityState.Detached;
                }
            }, cancellationToken);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
