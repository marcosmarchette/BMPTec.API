using FluentValidation;

namespace BMPTec.Application.Features.Contas.Commands.CadastrarConta
{
    public class CadastrarContaValidator : AbstractValidator<CadastrarContaCommand>
    {
        public CadastrarContaValidator()
        {
            RuleFor(x => x.UsuarioId)
                .NotEmpty().WithMessage("ID do usuário é obrigatório.");

            RuleFor(x => x.NumeroConta)
                .NotEmpty().WithMessage("Número da conta é obrigatório.")
                .MaximumLength(20).WithMessage("Número da conta não pode ter mais de 20 caracteres.");

            RuleFor(x => x.Agencia)
                .GreaterThan(0).WithMessage("Agência deve ser maior que zero.");

            RuleFor(x => x.TipoConta)
                .NotEmpty().WithMessage("Tipo de conta é obrigatório.")
                .Must(BeValidTipoConta).WithMessage("Tipo de conta inválido. Use: Corrente, Poupanca, Salario ou Investimento.");

            RuleFor(x => x.SaldoInicial)
                .GreaterThanOrEqualTo(0).WithMessage("Saldo inicial não pode ser negativo.");

            RuleFor(x => x.LimiteChequeEspecial)
                .GreaterThanOrEqualTo(0).WithMessage("Limite de cheque especial não pode ser negativo.");
        }

        private bool BeValidTipoConta(string tipoConta)
        {
            return tipoConta is "Corrente" or "Poupanca" or "Salario" or "Investimento";
        }
    }
}
