using BMPTec.Application.Features.Transferencias.Commands.RealizarTransferencia;
using FluentValidation;

namespace BMPTec.Application.Features.Transferencias.Validators
{
    public class RealizarTransferenciaValidator : AbstractValidator<RealizarTransferenciaCommand>
    {
        public RealizarTransferenciaValidator()
        {
            RuleFor(x => x.ContaOrigemId)
                .NotEmpty().WithMessage("ID da conta de origem é obrigatório.");

            RuleFor(x => x.ContaDestinoId)
                .NotEmpty().WithMessage("ID da conta de destino é obrigatório.")
                .NotEqual(x => x.ContaOrigemId).WithMessage("Conta de origem e destino não podem ser iguais.");

            RuleFor(x => x.Valor)
                .GreaterThan(0).WithMessage("Valor da transferência deve ser maior que zero.");

            RuleFor(x => x.Descricao)
                .MaximumLength(200).WithMessage("Descrição não pode ter mais de 200 caracteres.");

            RuleFor(x => x.TipoTransferencia)
                .NotEmpty().WithMessage("Tipo de transferência é obrigatório.")
                .Must(BeValidTipoTransferencia).WithMessage("Tipo de transferência inválido. Use: TED, DOC, PIX ou Interna.");
        }

        private bool BeValidTipoTransferencia(string tipoTransferencia)
        {
            return tipoTransferencia is "TED" or "DOC" or "PIX" or "Interna";
        }
    }
}
