using BMPTec.Domain.Common;
using BMPTec.Domain.Enums;
using BMPTec.Domain.Exceptions;
using BMPTec.Domain.ValueObject;

namespace BMPTec.Domain.Entities
{
    public class Conta : AuditableEntity
    {
        public Guid UsuarioId { get; private set; }
        public NumeroConta NumeroConta { get; private set; }
        public decimal Saldo { get; private set; }
        public TipoConta TipoConta { get; private set; }
        public DateTime DataAbertura { get; private set; }
        public DateTime? DataEncerramento { get; private set; }
        public bool Ativa { get; private set; }
        public decimal LimiteChequeEspecial { get; private set; }

      
        public Usuario Usuario { get; private set; } = null!;

      
        private Conta() : base()
        {
            NumeroConta = null!;
        }

        private Conta(Guid usuarioId, NumeroConta numeroConta, TipoConta tipoConta, decimal saldoInicial, decimal limiteChequeEspecial)
            : base()
        {
            UsuarioId = usuarioId;
            NumeroConta = numeroConta;
            TipoConta = tipoConta;
            Saldo = saldoInicial;
            LimiteChequeEspecial = limiteChequeEspecial;
            DataAbertura = DateTime.UtcNow;
            Ativa = true;

            ValidarSaldoInicial(saldoInicial);
            ValidarLimiteChequeEspecial(limiteChequeEspecial);
        }

        public static Conta Criar(Guid usuarioId, string numeroConta, int agencia, TipoConta tipoConta, decimal saldoInicial = 0, decimal limiteChequeEspecial = 0)
        {
            var numeroContaVO = NumeroConta.Criar(numeroConta, agencia);
            return new Conta(usuarioId, numeroContaVO, tipoConta, saldoInicial, limiteChequeEspecial);
        }

        public void Creditar(decimal valor)
        {
            ValidarOperacao();
            ValidarValor(valor, "crédito");

            Saldo += valor;
            MarcarComoAtualizado();
        }

        public void Debitar(decimal valor)
        {
            ValidarOperacao();
            ValidarValor(valor, "débito");
            ValidarSaldoSuficiente(valor);

            Saldo -= valor;
            MarcarComoAtualizado();
        }

        public void DefinirLimiteChequeEspecial(decimal limite)
        {
            ValidarOperacao();
            ValidarLimiteChequeEspecial(limite);

            LimiteChequeEspecial = limite;
            MarcarComoAtualizado();
        }

        public void Encerrar()
        {
            ValidarOperacao();

            if (Saldo != 0)
                throw new ContaException("Não é possível encerrar uma conta com saldo diferente de zero.");

            Ativa = false;
            DataEncerramento = DateTime.UtcNow;
            MarcarComoAtualizado();
        }

        public decimal ObterSaldoDisponivel()
        {
            return Saldo + LimiteChequeEspecial;
        }

        private void ValidarOperacao()
        {
            if (!Ativa)
                throw new ContaException("Operação não permitida. Conta está inativa.");
        }

        private static void ValidarValor(decimal valor, string tipoOperacao)
        {
            if (valor <= 0)
                throw new ContaException($"Valor de {tipoOperacao} deve ser maior que zero.");
        }

        private void ValidarSaldoSuficiente(decimal valor)
        {
            if (Saldo + LimiteChequeEspecial < valor)
                throw new ContaException("Saldo insuficiente para realizar a operação.");
        }

        private static void ValidarSaldoInicial(decimal saldo)
        {
            if (saldo < 0)
                throw new ContaException("Saldo inicial não pode ser negativo.");
        }

        private static void ValidarLimiteChequeEspecial(decimal limite)
        {
            if (limite < 0)
                throw new ContaException("Limite de cheque especial não pode ser negativo.");
        }
    }
}