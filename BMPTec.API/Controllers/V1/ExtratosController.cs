using Asp.Versioning;
using BMPTec.API.Middleware;
using BMPTec.Application.Features.Extratos.Queries.GerarExtrato;
using BMPTec.Application.Features.Extratos.Queries.ObterExtratoConsolidado;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BMPTec.API.Controllers.V1
{
    /// <summary>
    /// Endpoints para gerenciamento de extratos bancários
    /// </summary>
    [ApiVersion("1.0")]
    public class ExtratosController : BaseApiController
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ExtratosController> _logger;

        public ExtratosController(IMediator mediator, ILogger<ExtratosController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Gera extrato de uma conta por período
        /// </summary>
        /// <param name="contaId">ID da conta</param>
        /// <param name="dataInicio">Data de início do período</param>
        /// <param name="dataFim">Data de fim do período</param>
        /// <param name="cancellationToken">Token de cancelamento</param>
        /// <returns>Lista de movimentações do extrato</returns>
        /// <response code="200">Extrato gerado com sucesso</response>
        /// <response code="400">Erro de validação (ex: período inválido)</response>
        /// <response code="404">Conta não encontrada</response>
        /// <remarks>
        /// Exemplo de requisição:
        /// 
        ///     GET /api/v1/extratos/3fa85f64-5717-4562-b3fc-2c963f66afa6?dataInicio=2025-01-01&amp;dataFim=2025-01-31
        ///     
        /// Observações:
        /// - O período máximo de consulta é de 90 dias
        /// - As datas devem estar no formato ISO 8601 (YYYY-MM-DD)
        /// </remarks>
        [HttpGet("{contaId:guid}")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GerarExtrato(
            Guid contaId,
            [FromQuery] DateTime dataInicio,
            [FromQuery] DateTime dataFim,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Gerando extrato da conta {ContaId} de {DataInicio} até {DataFim}",
                contaId,
                dataInicio,
                dataFim);

            var query = new GerarExtratoQuery
            {
                ContaId = contaId,
                DataInicio = dataInicio,
                DataFim = dataFim
            };

            var result = await _mediator.Send(query, cancellationToken);

            if (result.Sucesso)
            {
                _logger.LogInformation("Extrato gerado com {Total} registros", result.Data?.Count ?? 0);
            }

            return HandleResult(result);
        }
        /// <summary>
        /// Obtém extrato consolidado de uma conta
        /// </summary>
        [HttpGet("{contaId:guid}/consolidado")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObterExtratoConsolidado(
            Guid contaId,
            [FromQuery] DateTime? dataInicio,
            [FromQuery] DateTime? dataFim,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Obtendo extrato consolidado da conta {ContaId}",
                contaId);

            var query = new ObterExtratoConsolidadoQuery
            {
                ContaId = contaId,
                DataInicio = dataInicio,
                DataFim = dataFim
            };

            var result = await _mediator.Send(query, cancellationToken);

            return HandleResult(result);
        }
    }
}
