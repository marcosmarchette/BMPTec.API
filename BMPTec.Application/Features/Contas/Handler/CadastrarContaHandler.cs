using AutoMapper;
using BMPTec.Application.Common;
using BMPTec.Application.DTOs;
using BMPTec.Application.Features.Contas.Commands.CadastrarConta;
using BMPTec.Domain.Interfaces;
using MediatR;
using BMPTec.Domain.Enums;
using BMPTec.Domain.Entities;

namespace BMPTec.Application.Features.Contas.Handler
{
    public class CadastrarContaHandler : IRequestHandler<CadastrarContaCommand, Result<ContaDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CadastrarContaHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<ContaDto>> Handle(CadastrarContaCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Verifica se o usuário existe
                var usuario = await _unitOfWork.Usuarios.ObterPorIdAsync(request.UsuarioId, cancellationToken);
                if (usuario == null)
                    return Result<ContaDto>.Failure("Usuário não encontrado.");

                if (!usuario.Ativo)
                    return Result<ContaDto>.Failure("Usuário está inativo.");

                // Verifica se o número da conta já existe
                var contaExiste = await _unitOfWork.Contas.NumeroContaExisteAsync(
                    request.NumeroConta,
                    request.Agencia,
                    cancellationToken);

                if (contaExiste)
                    return Result<ContaDto>.Failure("Número de conta já existe para esta agência.");

                // Converte o tipo de conta
                if (!Enum.TryParse<TipoConta>(request.TipoConta, out var tipoConta))
                    return Result<ContaDto>.Failure("Tipo de conta inválido.");

                // Cria a conta
                var conta = Conta.Criar(
                    request.UsuarioId,
                    request.NumeroConta,
                    request.Agencia,
                    tipoConta,
                    request.SaldoInicial,
                    request.LimiteChequeEspecial
                );

                await _unitOfWork.Contas.AdicionarAsync(conta, cancellationToken);

                // Se houver saldo inicial, cria um extrato de depósito
                if (request.SaldoInicial > 0)
                {
                    var extrato = Extrato.Criar(
                        conta.Id,
                        TipoOperacao.Deposito,
                        request.SaldoInicial,
                        0,
                        request.SaldoInicial,
                        "Depósito inicial - Abertura de conta"
                    );

                    await _unitOfWork.Extratos.AdicionarAsync(extrato, cancellationToken);
                }

                await _unitOfWork.CommitAsync(cancellationToken);

                var contaDto = _mapper.Map<ContaDto>(conta);
                contaDto.UsuarioId = usuario.Id;
                contaDto.NomeUsuario = usuario.Nome;
                contaDto.Email = usuario.Email.Endereco;
                contaDto.CPF = usuario.CPF.ToString();
                contaDto.Telefone = usuario.Telefone;

                return Result<ContaDto>.Success(contaDto, "Conta cadastrada com sucesso.");
            }
            catch (Exception ex)
            {
                return Result<ContaDto>.Failure($"Erro ao cadastrar conta: {ex.Message}");
            }
        }
    }
}
