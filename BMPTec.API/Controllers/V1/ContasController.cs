using Asp.Versioning;
using BMPTec.API.Middleware;
using BMPTec.Application.Features.Contas.Commands.CadastrarConta;
using BMPTec.Application.Features.Contas.Queries.ObterContaDetalhada;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BMPTec.API.Controllers.V1
{

    /// <summary>
    /// Endpoints para gerenciamento de contas bancárias
    /// </summary>
    [ApiVersion("1.0")]
    public class ContasController : BaseApiController
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ContasController> _logger;

        public ContasController(IMediator mediator, ILogger<ContasController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Cadastra uma nova conta bancária
        /// </summary>
        /// <param name="command">Dados da conta a ser cadastrada</param>
        /// <param name="cancellationToken">Token de cancelamento</param>
        /// <returns>Dados da conta criada</returns>
        /// <response code="200">Conta cadastrada com sucesso</response>
        /// <response code="400">Erro de validação</response>
        /// <response code="500">Erro interno do servidor</response>
        [HttpPost]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CadastrarConta(
            [FromBody] CadastrarContaCommand command,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Iniciando cadastro de conta para usuário {UsuarioId}", command.UsuarioId);

            var result = await _mediator.Send(command, cancellationToken);

            if (result.Sucesso)
            {
                _logger.LogInformation("Conta {ContaId} cadastrada com sucesso", result.Data?.ContaId);
            }

            return HandleResult(result);
        }

        /// <summary>
        /// Obtém uma conta por ID
        /// </summary>
        /// <param name="id">ID da conta</param>
        /// <param name="cancellationToken">Token de cancelamento</param>
        /// <returns>Dados da conta</returns>
        /// <response code="200">Conta encontrada</response>
        /// <response code="404">Conta não encontrada</response>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObterContaPorId(Guid id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Consultando conta {ContaId}", id);

            // TODO: Implementar query ObterContaPorId
            return Ok(new { mensagem = "Endpoint em desenvolvimento" });
        }

        /// <summary>
        /// Lista todas as contas de um usuário
        /// </summary>
        /// <param name="usuarioId">ID do usuário</param>
        /// <param name="cancellationToken">Token de cancelamento</param>
        /// <returns>Lista de contas do usuário</returns>
        /// <response code="200">Lista de contas retornada</response>
        [HttpGet("usuario/{usuarioId:guid}")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        public async Task<IActionResult> ListarContasPorUsuario(Guid usuarioId, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Listando contas do usuário {UsuarioId}", usuarioId);

            // TODO: Implementar query ListarContasPorUsuario
            return Ok(new { mensagem = "Endpoint em desenvolvimento" });
        }

        /// <summary>
        /// Obtém informações detalhadas de uma conta
        /// </summary>
        [HttpGet("{id:guid}/detalhada")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObterContaDetalhada(
            Guid id,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Obtendo detalhes da conta {ContaId}", id);

            var query = new ObterContaDetalhadaQuery { ContaId = id };
            var result = await _mediator.Send(query, cancellationToken);

            return HandleResult(result);
        }
    }
}
