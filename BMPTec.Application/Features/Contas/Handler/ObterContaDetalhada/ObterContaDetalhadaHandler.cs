using BMPTec.Application.Common;
using BMPTec.Application.Features.Contas.Queries.ObterContaDetalhada;
using BMPTec.Domain.Enums;
using BMPTec.Domain.Interfaces;
using MediatR;

namespace BMPTec.Application.Features.Contas.Handler.ObterContaDetalhada
{
    public class ObterContaDetalhadaHandler : IRequestHandler<ObterContaDetalhadaQuery, Result<ContaDetalhadaDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ObterContaDetalhadaHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<ContaDetalhadaDto>> Handle(ObterContaDetalhadaQuery request, CancellationToken cancellationToken)
        {
            var conta = await _unitOfWork.Contas.ObterPorIdAsync(request.ContaId, cancellationToken);

            if (conta == null)
                return Result<ContaDetalhadaDto>.Failure("Conta não encontrada.");

            // Busca transferências enviadas aprovadas
            var transferenciasEnviadas = await _unitOfWork.Transferencias
                .ObterPorContaOrigemAsync(request.ContaId, cancellationToken);
            var transferenciasEnviadasAprovadas = transferenciasEnviadas
                .Where(t => t.Status == StatusTransferencia.Aprovada);

            // Busca transferências recebidas aprovadas
            var transferenciasRecebidas = await _unitOfWork.Transferencias
                .ObterPorContaDestinoAsync(request.ContaId, cancellationToken);
            var transferenciasRecebidasAprovadas = transferenciasRecebidas
                .Where(t => t.Status == StatusTransferencia.Aprovada);

            // Busca usuário
            var usuario = await _unitOfWork.Usuarios.ObterPorIdAsync(conta.UsuarioId, cancellationToken);

            if (usuario == null)
                return Result<ContaDetalhadaDto>.Failure("Usuário da conta não encontrado.");

            var contaDetalhada = new ContaDetalhadaDto
            {
                ContaId = conta.Id,
                NumeroConta = conta.NumeroConta.Numero,
                Agencia = conta.NumeroConta.Agencia,
                Saldo = conta.Saldo,
                TipoConta = conta.TipoConta.ToString(),
                LimiteChequeEspecial = conta.LimiteChequeEspecial,
                SaldoDisponivel = conta.ObterSaldoDisponivel(),
                DataAbertura = conta.DataAbertura,
                Ativa = conta.Ativa,
                UsuarioId = usuario.Id,
                NomeUsuario = usuario.Nome,
                Email = usuario.Email.ToString(),
                CPF = usuario.CPF.Numero,
                Telefone = usuario.Telefone,
                TotalTransferenciasEnviadas = transferenciasEnviadasAprovadas.Count(),
                TotalTransferenciasRecebidas = transferenciasRecebidasAprovadas.Count(),
                ValorTotalEnviado = transferenciasEnviadasAprovadas.Sum(t => t.Valor),
                ValorTotalRecebido = transferenciasRecebidasAprovadas.Sum(t => t.Valor)
            };

            return Result<ContaDetalhadaDto>.Success(contaDetalhada);
        }
    }
}
