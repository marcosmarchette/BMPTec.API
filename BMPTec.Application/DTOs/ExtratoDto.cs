namespace BMPTec.Application.DTOs
{
    public class ExtratoDto
    {
        public Guid Id { get; set; }
        public Guid ContaId { get; set; }
        public string TipoOperacao { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public decimal SaldoAnterior { get; set; }
        public decimal SaldoAtual { get; set; }
        public DateTime DataOperacao { get; set; }
        public string? Descricao { get; set; }
        public Guid? TransferenciaId { get; set; }
        public string? NumeroDocumento { get; set; }
    }
}
