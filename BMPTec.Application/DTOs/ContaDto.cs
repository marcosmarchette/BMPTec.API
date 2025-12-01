namespace BMPTec.Application.DTOs
{
    public class ContaDto
    {
        public Guid ContaId { get; set; }
        public string NumeroConta { get; set; } = string.Empty;
        public int Agencia { get; set; }
        public decimal Saldo { get; set; }
        public string TipoConta { get; set; } = string.Empty;
        public decimal LimiteChequeEspecial { get; set; }
        public decimal SaldoDisponivel { get; set; }
        public DateTime DataAbertura { get; set; }
        public bool Ativa { get; set; }

        // Dados do Usuário
        public Guid UsuarioId { get; set; }
        public string NomeUsuario { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string CPF { get; set; } = string.Empty;
        public string? Telefone { get; set; }

        // Estatísticas de Transferências
        public int TotalTransferenciasEnviadas { get; set; }
        public int TotalTransferenciasRecebidas { get; set; }
        public decimal ValorTotalEnviado { get; set; }
        public decimal ValorTotalRecebido { get; set; }
    }
}
