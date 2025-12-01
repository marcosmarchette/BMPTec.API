using BMPTec.Domain.Exceptions;
using System.Text.RegularExpressions;

namespace BMPTec.Domain.ValueObject
{
    public sealed class Cpf : Common.ValueObject
    {
        public string Numero { get; private set; }

        private Cpf(string numero)
        {
            Numero = numero;
        }

        public static Cpf Criar(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf))
                throw new DomainException("CPF não pode ser vazio.");

            // Remove caracteres não numéricos
            var cpfLimpo = Regex.Replace(cpf, @"[^\d]", "");

            if (cpfLimpo.Length != 11)
                throw new DomainException("CPF deve conter 11 dígitos.");

            if (!ValidarCPF(cpfLimpo))
                throw new DomainException("CPF inválido.");

            return new Cpf(cpfLimpo);
        }

        private static bool ValidarCPF(string cpf)
        {
            // Verifica se todos os dígitos são iguais
            if (cpf.All(c => c == cpf[0]))
                return false;

            // Calcula primeiro dígito verificador
            int soma = 0;
            for (int i = 0; i < 9; i++)
                soma += int.Parse(cpf[i].ToString()) * (10 - i);

            int resto = soma % 11;
            int digitoVerificador1 = resto < 2 ? 0 : 11 - resto;

            if (int.Parse(cpf[9].ToString()) != digitoVerificador1)
                return false;

            // Calcula segundo dígito verificador
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(cpf[i].ToString()) * (11 - i);

            resto = soma % 11;
            int digitoVerificador2 = resto < 2 ? 0 : 11 - resto;

            return int.Parse(cpf[10].ToString()) == digitoVerificador2;
        }

        public string FormatarCPF()
        {
            return $"{Numero.Substring(0, 3)}.{Numero.Substring(3, 3)}.{Numero.Substring(6, 3)}-{Numero.Substring(9, 2)}";
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Numero;
        }

        public override string ToString() => FormatarCPF();
    }
}
