using BMPTec.Domain.Common;

namespace BMPTec.Domain.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<T?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<T>> ObterTodosAsync(CancellationToken cancellationToken = default);
        Task AdicionarAsync(T entidade, CancellationToken cancellationToken = default);
        void Atualizar(T entidade);
        void Remover(T entidade);
    }
}
