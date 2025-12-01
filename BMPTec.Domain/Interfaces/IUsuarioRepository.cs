using BMPTec.Domain.Entities;

namespace BMPTec.Domain.Interfaces
{
    public interface IUsuarioRepository : IRepository<Usuario>
    {
        Task<Usuario?> ObterPorEmailAsync(string email, CancellationToken cancellationToken = default);
        Task<Usuario?> ObterPorCPFAsync(string cpf, CancellationToken cancellationToken = default);
        Task<bool> EmailExisteAsync(string email, CancellationToken cancellationToken = default);
        Task<bool> CPFExisteAsync(string cpf, CancellationToken cancellationToken = default);
        Task<IEnumerable<Usuario>> ObterAtivosAsync(CancellationToken cancellationToken = default);
    }
}
