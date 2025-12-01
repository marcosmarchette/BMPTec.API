using BMPTec.Domain.Exceptions;
using System.Text.RegularExpressions;

namespace BMPTec.Domain.ValueObject
{
    public sealed class Email : Common.ValueObject
    {
        public string Endereco { get; private set; }

        private Email(string endereco)
        {
            Endereco = endereco;
        }

        public static Email Criar(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new DomainException("E-mail não pode ser vazio.");

            var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            if (!regex.IsMatch(email))
                throw new DomainException("E-mail inválido.");

            return new Email(email.ToLowerInvariant());
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Endereco;
        }

        public override string ToString() => Endereco;
    }
}
