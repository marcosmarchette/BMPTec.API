using Asp.Versioning;
using BMPTec.Application.Common;
using Microsoft.AspNetCore.Mvc;

namespace BMPTec.API.Controllers.V1
{
    /// <summary>
    /// Controller base para a API v1
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    public abstract class BaseApiController : ControllerBase
    {
        protected IActionResult HandleResult<T>(Result<T> result)
        {
            if (result.Sucesso)
            {
                return Ok(new
                {
                    sucesso = true,
                    mensagem = result.Mensagem,
                    data = result.Data
                });
            }

            return BadRequest(new
            {
                sucesso = false,
                erros = result.Erros
            });
        }

        protected IActionResult HandleResult(Result result)
        {
            if (result.Sucesso)
            {
                return Ok(new
                {
                    sucesso = true,
                    mensagem = result.Mensagem
                });
            }

            return BadRequest(new
            {
                sucesso = false,
                erros = result.Erros
            });
        }
    }
}
