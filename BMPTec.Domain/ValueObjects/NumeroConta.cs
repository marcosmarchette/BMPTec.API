using BMPTec.Domain.Exceptions;

namespace BMPTec.Domain.ValueObject
{
    public sealed class NumeroConta : Common.ValueObject
    {
        public string Numero { get; private set; }
        public int Agencia { get; private set; }

        private NumeroConta(string numero, int agencia)
        {
            Numero = numero;
            Agencia = agencia;
        }

        public static NumeroConta Criar(string numero, int agencia)
        {
            if (string.IsNullOrWhiteSpace(numero))
                throw new DomainException("Número da conta não pode ser vazio.");

            if (agencia <= 0)
                throw new DomainException("Agência deve ser maior que zero.");

            if (numero.Length > 20)
                throw new DomainException("Número da conta não pode ter mais de 20 caracteres.");

            return new NumeroConta(numero, agencia);
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Numero;
            yield return Agencia;
        }

        public override string ToString() => $"Ag: {Agencia:0000} - Conta: {Numero}";
    }
}
