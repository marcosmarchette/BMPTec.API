using BMPTec.Domain.Entities;

namespace BMPTec.Domain.Interfaces
{
    public interface IContaRepository : IRepository<Conta>
    {
        Task<Conta?> ObterPorNumeroContaAsync(string numeroConta, int agencia, CancellationToken cancellationToken = default);
        Task<IEnumerable<Conta>> ObterPorUsuarioIdAsync(Guid usuarioId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Conta>> ObterAtivasAsync(CancellationToken cancellationToken = default);
        Task<bool> NumeroContaExisteAsync(string numeroConta, int agencia, CancellationToken cancellationToken = default);
    }
}
