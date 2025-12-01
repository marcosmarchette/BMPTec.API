namespace BMPTec.Application.DTOs
{
    public class ExtratoConsolidadoDto
    {
        public Guid ExtratoId { get; set; }
        public Guid ContaId { get; set; }
        public string NumeroConta { get; set; } = string.Empty;
        public int Agencia { get; set; }
        public string Titular { get; set; } = string.Empty;
        public string TipoOperacao { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public decimal SaldoAnterior { get; set; }
        public decimal SaldoAtual { get; set; }
        public DateTime DataOperacao { get; set; }
        public string? Descricao { get; set; }
        public string? NumeroDocumento { get; set; }
        public Guid? TransferenciaId { get; set; }
        public string? ContaRelacionada { get; set; }
    }
}