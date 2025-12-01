using BMPTec.Domain.Common;
using BMPTec.Domain.Enums;
using BMPTec.Domain.Exceptions;

namespace BMPTec.Domain.Entities
{
    public class Extrato : BaseEntity
    {
        public Guid ContaId { get; private set; }
        public TipoOperacao TipoOperacao { get; private set; }
        public decimal Valor { get; private set; }
        public decimal SaldoAnterior { get; private set; }
        public decimal SaldoAtual { get; private set; }
        public DateTime DataOperacao { get; private set; }
        public string? Descricao { get; private set; }
        public Guid? TransferenciaId { get; private set; }
        public string? NumeroDocumento { get; private set; }

        // Navegação
        public Conta Conta { get; private set; } = null!;
        public Transferencia? Transferencia { get; set; }

        // Construtor privado para EF Core
        private Extrato() : base()
        {
        }

        private Extrato(Guid contaId, TipoOperacao tipoOperacao, decimal valor, decimal saldoAnterior, decimal saldoAtual, string? descricao, Guid? transferenciaId, string? numeroDocumento)
            : base()
        {
            ValidarExtrato(contaId, valor);

            ContaId = contaId;
            TipoOperacao = tipoOperacao;
            Valor = valor;
            SaldoAnterior = saldoAnterior;
            SaldoAtual = saldoAtual;
            DataOperacao = DateTime.UtcNow;
            Descricao = descricao;
            TransferenciaId = transferenciaId;
            NumeroDocumento = numeroDocumento;
        }

        public static Extrato Criar(Guid contaId, TipoOperacao tipoOperacao, decimal valor, decimal saldoAnterior, decimal saldoAtual, string? descricao = null, Guid? transferenciaId = null, string? numeroDocumento = null)
        {
            return new Extrato(contaId, tipoOperacao, valor, saldoAnterior, saldoAtual, descricao, transferenciaId, numeroDocumento);
        }

        private static void ValidarExtrato(Guid contaId, decimal valor)
        {
            if (contaId == Guid.Empty)
                throw new DomainException("ID da conta é inválido.");

            if (valor == 0)
                throw new DomainException("Valor da operação não pode ser zero.");
        }
    }
}
