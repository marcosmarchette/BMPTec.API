namespace BMPTec.Application.Features.Contas.Dto
{
    public class RelatorioContaDto
    {
        public int ContaId { get; set; }
        public string? NumeroConta { get; set; }
        public string? Agencia { get; set; }
        public string? NomeUsuario { get; set; }
        public decimal Saldo { get; set; }
        public decimal SaldoDisponivel { get; set; }
        public int TotalTransferenciasEnviadas { get; set; }
        public decimal ValorTotalEnviado { get; set; }
        // Add other properties as needed to match your projection
    }
}