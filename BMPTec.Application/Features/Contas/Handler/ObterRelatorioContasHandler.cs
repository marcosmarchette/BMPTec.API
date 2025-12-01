using BMPTec.Application.Common;
using BMPTec.Application.Features.Contas.Dto;
using BMPTec.Domain.Enums;
using BMPTec.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace BMPTec.Application.Features.Contas.Handler
{
    public class ObterRelatorioContasHandler
    {
        private readonly BmpTecContext _context;

        public ObterRelatorioContasHandler(BmpTecContext context)
        {
            _context = context;
        }

        public async Task<Result<List<RelatorioContaDto>>> Handle()
        {
            var relatorio = await _context.Contas
                .Include(c => c.Usuario)
                .Select(c => new RelatorioContaDto
                {
                    ContaId = c.Id.GetHashCode(), // Convert Guid to int using GetHashCode()
                    NumeroConta = c.NumeroConta.Numero,
                    Agencia = c.NumeroConta.Agencia.ToString(),
                    NomeUsuario = c.Usuario.Nome,
                    Saldo = c.Saldo,
                    SaldoDisponivel = c.Saldo + c.LimiteChequeEspecial                  
                })
                .ToListAsync();

            return Result<List<RelatorioContaDto>>.Success(relatorio);
        }
    }
}
