using Asp.Versioning;
using BMPTec.API.Middleware;
using BMPTec.Application.Features.Transferencias.Commands.RealizarTransferencia;
using BMPTec.Application.Features.Transferencias.Queries.ObterTransferenciaDetalhada;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BMPTec.API.Controllers.V1
{

    /// <summary>
    /// Endpoints para gerenciamento de transferências bancárias
    /// </summary>
    [ApiVersion("1.0")]
    public class TransferenciasController : BaseApiController
    {
        private readonly IMediator _mediator;
        private readonly ILogger<TransferenciasController> _logger;

        public TransferenciasController(IMediator mediator, ILogger<TransferenciasController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Realiza uma transferência entre contas
        /// </summary>
        /// <param name="command">Dados da transferência</param>
        /// <param name="cancellationToken">Token de cancelamento</param>
        /// <returns>Dados da transferência realizada</returns>
        /// <response code="200">Transferência realizada com sucesso</response>
        /// <response code="400">Erro de validação (ex: saldo insuficiente, dia não útil)</response>
        /// <response code="500">Erro interno do servidor</response>
        /// <remarks>
        /// Exemplo de requisição:
        /// 
        ///     POST /api/v1/transferencias
        ///     {
        ///         "contaOrigemId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///         "contaDestinoId": "3fa85f64-5717-4562-b3fc-2c963f66afa7",
        ///         "valor": 1000.00,
        ///         "descricao": "Pagamento",
        ///         "tipoTransferencia": "TED"
        ///     }
        ///     
        /// Observações importantes:
        /// - Transferências só podem ser realizadas em dias úteis
        /// - A conta de origem deve ter saldo suficiente
        /// - Ambas as contas devem estar ativas
        /// </remarks>
        [HttpPost]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RealizarTransferencia(
            [FromBody] RealizarTransferenciaCommand command,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Iniciando transferência de {Valor} da conta {ContaOrigemId} para {ContaDestinoId}",
                command.Valor,
                command.ContaOrigemId,
                command.ContaDestinoId);

            var result = await _mediator.Send(command, cancellationToken);

            if (result.Sucesso)
            {
                _logger.LogInformation("Transferência {TransferenciaId} realizada com sucesso", result.Data?.ContaOrigemId);
            }
            else
            {
                _logger.LogWarning("Falha na transferência: {Erros}", string.Join(", ", result.Erros));
            }

            return HandleResult(result);
        }

        /// <summary>
        /// Obtém uma transferência por ID
        /// </summary>
        /// <param name="id">ID da transferência</param>
        /// <param name="cancellationToken">Token de cancelamento</param>
        /// <returns>Dados da transferência</returns>
        /// <response code="200">Transferência encontrada</response>
        /// <response code="404">Transferência não encontrada</response>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObterTransferenciaPorId(Guid id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Consultando transferência {TransferenciaId}", id);

            // TODO: Implementar query ObterTransferenciaPorId
            return Ok(new { mensagem = "Endpoint em desenvolvimento" });
        }

        /// <summary>
        /// Lista transferências de uma conta
        /// </summary>
        /// <param name="contaId">ID da conta</param>
        /// <param name="cancellationToken">Token de cancelamento</param>
        /// <returns>Lista de transferências</returns>
        /// <response code="200">Lista de transferências retornada</response>
        [HttpGet("conta/{contaId:guid}")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        public async Task<IActionResult> ListarTransferenciasPorConta(Guid contaId, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Listando transferências da conta {ContaId}", contaId);

            // TODO: Implementar query ListarTransferenciasPorConta
            return Ok(new { mensagem = "Endpoint em desenvolvimento" });
        }


        /// <summary>
        /// Obtém informações detalhadas de uma transferência
        /// </summary>
        [HttpGet("{id:guid}/detalhada")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObterTransferenciaDetalhada(
            Guid id,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Obtendo detalhes da transferência {TransferenciaId}", id);

            var query = new ObterTransferenciaDetalhadaQuery { TransferenciaId = id };
            var result = await _mediator.Send(query, cancellationToken);

            return HandleResult(result);
        }
    }
}
