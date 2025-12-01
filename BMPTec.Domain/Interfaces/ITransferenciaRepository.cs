using BMPTec.Domain.Entities;
using BMPTec.Domain.Enums;

namespace BMPTec.Domain.Interfaces
{
    public interface ITransferenciaRepository : IRepository<Transferencia>
    {
        Task<IEnumerable<Transferencia>> ObterPorContaOrigemAsync(Guid contaOrigemId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Transferencia>> ObterPorContaDestinoAsync(Guid contaDestinoId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Transferencia>> ObterPorStatusAsync(StatusTransferencia status, CancellationToken cancellationToken = default);
        Task<IEnumerable<Transferencia>> ObterPorPeriodoAsync(DateTime dataInicio, DateTime dataFim, CancellationToken cancellationToken = default);
        Task<IEnumerable<Transferencia>> ObterPorContaEPeriodoAsync(Guid contaId, DateTime dataInicio, DateTime dataFim, CancellationToken cancellationToken = default);

    }
}
