namespace BMPTec.Domain.Common
{
    public abstract class AuditableEntity : BaseEntity
    {
        public DateTime DataCriacao { get; protected set; }
        public DateTime? DataAtualizacao { get; protected set; }

        protected AuditableEntity() : base()
        {
            DataCriacao = DateTime.UtcNow;
        }

        protected AuditableEntity(Guid id) : base(id)
        {
            DataCriacao = DateTime.UtcNow;
        }

        public void MarcarComoAtualizado()
        {
            DataAtualizacao = DateTime.UtcNow;
        }
    }
}
