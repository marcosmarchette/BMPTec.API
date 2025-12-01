using BMPTec.Domain.Entities;

namespace BMPTec.Domain.Interfaces
{
    public interface IFeriadoRepository : IRepository<Feriado>
    {
        Task<Feriado?> ObterPorDataAsync(DateTime data, CancellationToken cancellationToken = default);
        Task<IEnumerable<Feriado>> ObterPorAnoAsync(int ano, CancellationToken cancellationToken = default);
        Task<bool> DataEhFeriadoAsync(DateTime data, CancellationToken cancellationToken = default);
    }
}
