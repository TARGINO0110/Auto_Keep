using Auto_Keep.Models.AutoKeep;
using Auto_Keep.Services.ServiceEstoqueMonetario.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auto_Keep.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize(Roles = "Administrador")]
    public class EstoqueMonetarioController : ControllerBase
    {
        private readonly IEstoqueMonetarioRepository _estoqueMonetarioRepository;
        public EstoqueMonetarioController(IEstoqueMonetarioRepository estoqueMonetarioRepository)
        {
            _estoqueMonetarioRepository = estoqueMonetarioRepository;
        }

        /// <summary>
        /// Obter lista de estoque monetário
        /// </summary>
        /// <returns></returns>
        [HttpGet("ListarEstoqueGeral")]
        public async Task<ActionResult<IEnumerable<EstoqueMonetario>>> ListarEstoqueGeral()
        {
            var response = await _estoqueMonetarioRepository.GetEstoqueGeral();
            return Ok(response);
        }

        /// <summary>
        /// Registrar novo estoque monetário
        /// </summary>
        /// <param name="postEstoqueMonetario"></param>
        /// <returns></returns>
        [HttpPost("RegistrarEstoque")]
        public async Task<ActionResult> RegistrarEstoque([FromBody] PostEstoqueMonetario postEstoqueMonetario)
        {
            await _estoqueMonetarioRepository.PostEstoque(postEstoqueMonetario);
            return Ok();
        }

        /// <summary>
        /// Filtrar por id do estoque
        /// </summary>
        /// <param name="idEstoque">Id do estoque</param>
        [HttpGet("FiltrarIdEstoque/{idEstoque}")]
        public async Task<ActionResult<EstoqueMonetario>> FiltrarIdEstoque(int idEstoque)
        {
            var response = await _estoqueMonetarioRepository.GetById(idEstoque);
            return Ok(response);
        }

        /// <summary>
        /// Editar estoque monetário
        /// </summary>
        /// <param name="idEstoque">Id do estoque</param>
        /// <param name="putEstoqueMonetario"></param>
        [HttpPut("EditarEstoque/{idEstoque}")]
        public async Task<ActionResult<EstoqueMonetario>> EditarEstoque(int idEstoque, [FromBody] PutEstoqueMonetario putEstoqueMonetario)
        {
            var response = await _estoqueMonetarioRepository.PutEstoque(idEstoque, putEstoqueMonetario);
            return Ok(response);
        }

        /// <summary>
        /// Atualizar Quatidade do estoque monetário
        /// </summary>
        /// <param name="idEstoque">Id do estoque</param>
        /// <param name="qtd"></param>
        [HttpPut("AtualizarQtdEstoque/{idEstoque},{qtd}")]
        public async Task<ActionResult<EstoqueMonetario>> AtualizarQtdEstoque(int idEstoque, int qtd)
        {
            var response = await _estoqueMonetarioRepository.PutQtdEstoque(idEstoque, qtd);
            return Ok(response);
        }

        /// <summary>
        /// Remover referência da moeda ou cedula do estoque
        /// </summary>
        /// <param name="idEstoque">Id do estoque</param>
        [HttpDelete("RemoverEstoque/{idEstoque}")]
        public async Task<ActionResult<int>> RemoverEstoque(int idEstoque)
        {
            var response = await _estoqueMonetarioRepository.DeleteEstoque(idEstoque);
            return Ok(response);
        }
    }
}
