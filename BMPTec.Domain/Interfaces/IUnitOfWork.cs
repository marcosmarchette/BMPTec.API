namespace BMPTec.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUsuarioRepository Usuarios { get; }
        IContaRepository Contas { get; }
        ITransferenciaRepository Transferencias { get; }
        IExtratoRepository Extratos { get; }
        IFeriadoRepository Feriados { get; }

        Task<int> CommitAsync(CancellationToken cancellationToken = default);
        Task RollbackAsync(CancellationToken cancellationToken = default);
    }
}
