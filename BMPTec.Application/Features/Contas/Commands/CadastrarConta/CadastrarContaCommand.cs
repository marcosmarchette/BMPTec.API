using BMPTec.Application.Common;
using BMPTec.Application.DTOs;
using MediatR;

namespace BMPTec.Application.Features.Contas.Commands.CadastrarConta
{
    public record CadastrarContaCommand : IRequest<Result<ContaDto>>
    {
        public Guid UsuarioId { get; init; }
        public string NumeroConta { get; init; } = string.Empty;
        public int Agencia { get; init; }
        public string TipoConta { get; init; } = string.Empty;
        public decimal SaldoInicial { get; init; }
        public decimal LimiteChequeEspecial { get; init; }
    }
}
