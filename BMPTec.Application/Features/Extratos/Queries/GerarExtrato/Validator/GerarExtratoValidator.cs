using FluentValidation;

namespace BMPTec.Application.Features.Extratos.Queries.GerarExtrato.Validator
{
    public class GerarExtratoValidator : AbstractValidator<GerarExtratoQuery>
    {
        public GerarExtratoValidator()
        {
            RuleFor(x => x.ContaId)
                .NotEmpty().WithMessage("ID da conta é obrigatório.");

            RuleFor(x => x.DataInicio)
                .NotEmpty().WithMessage("Data de início é obrigatória.")
                .LessThanOrEqualTo(x => x.DataFim).WithMessage("Data de início deve ser anterior ou igual à data de fim.");

            RuleFor(x => x.DataFim)
                .NotEmpty().WithMessage("Data de fim é obrigatória.")
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Data de fim não pode ser futura.");

            RuleFor(x => x)
                .Must(x => (x.DataFim - x.DataInicio).TotalDays <= 90)
                .WithMessage("O período de consulta não pode ser superior a 90 dias.");
        }
    }
}
