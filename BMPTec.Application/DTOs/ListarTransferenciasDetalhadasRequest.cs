namespace BMPTec.Application.Features.Transferencias
{
    public class ListarTransferenciasDetalhadasRequest
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? Status { get; set; }
        public DateTime? DataInicio { get; set; }
    }
}