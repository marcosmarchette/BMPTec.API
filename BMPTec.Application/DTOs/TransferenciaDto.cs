using System;
using System.Collections.Generic;
using System.Text;

namespace BMPTec.Application.DTOs
{
    public class TransferenciaDto
    {
        public Guid TransferenciaId { get; set; }
        public decimal Valor { get; set; }
        public string? Descricao { get; set; }
        public DateTime DataTransferencia { get; set; }
        public DateTime? DataProcessamento { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? MotivoRejeicao { get; set; }
        public string TipoTransferencia { get; set; } = string.Empty;

        // Conta Origem
        public Guid ContaOrigemId { get; set; }
        public string ContaOrigemNumero { get; set; } = string.Empty;
        public int ContaOrigemAgencia { get; set; }
        public string ContaOrigemTitular { get; set; } = string.Empty;

        // Conta Destino
        public Guid ContaDestinoId { get; set; }
        public string ContaDestinoNumero { get; set; } = string.Empty;
        public int ContaDestinoAgencia { get; set; }
        public string ContaDestinoTitular { get; set; } = string.Empty;
    }
}
