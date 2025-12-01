using BMPTec.Domain.Entities;

namespace BMPTec.Domain.Interfaces
{
    public interface IExtratoRepository : IRepository<Extrato>
    {
        Task<IEnumerable<Extrato>> ObterPorContaAsync(Guid contaId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Extrato>> ObterPorContaEPeriodoAsync(Guid contaId, DateTime dataInicio, DateTime dataFim, CancellationToken cancellationToken = default);
        Task<IEnumerable<Extrato>> ObterPorTransferenciaAsync(Guid transferenciaId, CancellationToken cancellationToken = default);
    }
}
