namespace BMPTec.Application.Features.Contas.Queries.ObterContaDetalhada
{
    public class ContaDetalhadaDto
    {
        public Guid ContaId { get; set; }
        public string? NumeroConta { get; set; }
        public int Agencia { get; set; }
        public decimal Saldo { get; set; }
        public string? TipoConta { get; set; }
        public decimal LimiteChequeEspecial { get; set; }
        public decimal SaldoDisponivel { get; set; }
        public DateTime DataAbertura { get; set; }
        public bool Ativa { get; set; }
        public Guid UsuarioId { get; set; }
        public string? NomeUsuario { get; set; }
        public string? Email { get; set; }
        public string? CPF { get; set; }
        public string? Telefone { get; set; }
        public int TotalTransferenciasEnviadas { get; set; }
        public int TotalTransferenciasRecebidas { get; set; }
        public decimal ValorTotalEnviado { get; set; }
        public decimal ValorTotalRecebido { get; set; }
    }
}