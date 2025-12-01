using BMPTec.Domain.Common;
using BMPTec.Domain.Exceptions;
using BMPTec.Domain.ValueObject;

namespace BMPTec.Domain.Entities
{
    public class Usuario : AuditableEntity
    {
        public string Nome { get; private set; }
        public Email Email { get; private set; }
        public Cpf CPF { get; private set; }
        public string? Telefone { get; private set; }
        public DateTime? DataNascimento { get; private set; }
        public bool Ativo { get; private set; }

        // Navegação
        private readonly List<Conta> _contas = new();
        public IReadOnlyCollection<Conta> Contas => _contas.AsReadOnly();

        // Construtor privado para EF Core
        private Usuario() : base()
        {
            Nome = string.Empty;
            Email = null!;
            CPF = null!;
        }

        private Usuario(string nome, Email email, Cpf cpf, string? telefone, DateTime? dataNascimento)
            : base()
        {
            ValidarNome(nome);

            Nome = nome;
            Email = email;
            CPF = cpf;
            Telefone = telefone;
            DataNascimento = dataNascimento;
            Ativo = true;
        }

        public static Usuario Criar(string nome, string email, string cpf, string? telefone = null, DateTime? dataNascimento = null)
        {
            var emailVO = Email.Criar(email);
            var cpfVO = Cpf.Criar(cpf);

            if (dataNascimento.HasValue && dataNascimento.Value >= DateTime.Today)
                throw new DomainException("Data de nascimento deve ser anterior à data atual.");

            return new Usuario(nome, emailVO, cpfVO, telefone, dataNascimento);
        }

        public void AtualizarDados(string nome, string? telefone, DateTime? dataNascimento)
        {
            ValidarNome(nome);

            if (dataNascimento.HasValue && dataNascimento.Value >= DateTime.Today)
                throw new DomainException("Data de nascimento deve ser anterior à data atual.");

            Nome = nome;
            Telefone = telefone;
            DataNascimento = dataNascimento;
            MarcarComoAtualizado();
        }

        public void Inativar()
        {
            if (!Ativo)
                throw new DomainException("Usuário já está inativo.");

            Ativo = false;
            MarcarComoAtualizado();
        }

        public void Ativar()
        {
            if (Ativo)
                throw new DomainException("Usuário já está ativo.");

            Ativo = true;
            MarcarComoAtualizado();
        }

        private static void ValidarNome(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new DomainException("Nome não pode ser vazio.");

            if (nome.Length < 3)
                throw new DomainException("Nome deve ter no mínimo 3 caracteres.");

            if (nome.Length > 100)
                throw new DomainException("Nome não pode ter mais de 100 caracteres.");
        }
    }
}
