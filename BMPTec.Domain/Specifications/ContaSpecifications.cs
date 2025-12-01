using BMPTec.Domain.Entities;
using System.Linq.Expressions;

namespace BMPTec.Domain.Specifications
{
    public static class ContaSpecifications
    {
        public static Expression<Func<Conta, bool>> ContasAtivas()
            => c => c.Ativa;

        public static Expression<Func<Conta, bool>> ContasPorUsuario(Guid usuarioId)
            => c => c.UsuarioId == usuarioId && c.Ativa;
    }
}
