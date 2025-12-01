using BMPTec.Domain.Common;
using BMPTec.Domain.Enums;
using BMPTec.Domain.Exceptions;

namespace BMPTec.Domain.Entities
{
    public class Transferencia : BaseEntity
    {
        public Guid ContaOrigemId { get; private set; }
        public Guid ContaDestinoId { get; private set; }
        public decimal Valor { get; private set; }
        public string? Descricao { get; private set; }
        public DateTime DataTransferencia { get; private set; }
        public DateTime? DataProcessamento { get; private set; }
        public StatusTransferencia Status { get; private set; }
        public string? MotivoRejeicao { get; private set; }
        public TipoTransferencia TipoTransferencia { get; private set; }

        // Navegação
        public Conta ContaOrigem { get; private set; } = null!;
        public Conta ContaDestino { get; private set; } = null!;

        // Construtor privado para EF Core
        private Transferencia() : base()
        {
        }

        private Transferencia(Guid contaOrigemId, Guid contaDestinoId, decimal valor, string? descricao, TipoTransferencia tipoTransferencia)
            : base()
        {
            ValidarTransferencia(contaOrigemId, contaDestinoId, valor);

            ContaOrigemId = contaOrigemId;
            ContaDestinoId = contaDestinoId;
            Valor = valor;
            Descricao = descricao;
            TipoTransferencia = tipoTransferencia;
            DataTransferencia = DateTime.UtcNow;
            Status = StatusTransferencia.Pendente;
        }

        public static Transferencia Criar(Guid contaOrigemId, Guid contaDestinoId, decimal valor, string? descricao = null, TipoTransferencia tipoTransferencia = TipoTransferencia.TED)
        {
            return new Transferencia(contaOrigemId, contaDestinoId, valor, descricao, tipoTransferencia);
        }

        public void Aprovar()
        {
            if (Status != StatusTransferencia.Pendente)
                throw new TransferenciaException($"Não é possível aprovar uma transferência com status {Status}.");

            Status = StatusTransferencia.Aprovada;
            DataProcessamento = DateTime.UtcNow;
        }

        public void Rejeitar(string motivo)
        {
            if (Status != StatusTransferencia.Pendente)
                throw new TransferenciaException($"Não é possível rejeitar uma transferência com status {Status}.");

            if (string.IsNullOrWhiteSpace(motivo))
                throw new TransferenciaException("Motivo da rejeição deve ser informado.");

            Status = StatusTransferencia.Rejeitada;
            MotivoRejeicao = motivo;
            DataProcessamento = DateTime.UtcNow;
        }

        public void Cancelar()
        {
            if (Status == StatusTransferencia.Aprovada)
                throw new TransferenciaException("Não é possível cancelar uma transferência já aprovada.");

            if (Status == StatusTransferencia.Cancelada)
                throw new TransferenciaException("Transferência já está cancelada.");

            Status = StatusTransferencia.Cancelada;
            DataProcessamento = DateTime.UtcNow;
        }

        private static void ValidarTransferencia(Guid contaOrigemId, Guid contaDestinoId, decimal valor)
        {
            if (contaOrigemId == Guid.Empty)
                throw new TransferenciaException("ID da conta de origem é inválido.");

            if (contaDestinoId == Guid.Empty)
                throw new TransferenciaException("ID da conta de destino é inválido.");

            if (contaOrigemId == contaDestinoId)
                throw new TransferenciaException("Conta de origem e destino não podem ser iguais.");

            if (valor <= 0)
                throw new TransferenciaException("Valor da transferência deve ser maior que zero.");
        }
    }
}
