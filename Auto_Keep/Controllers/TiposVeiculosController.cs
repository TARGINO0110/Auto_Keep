using Auto_Keep.Models.AutoKeep;
using Auto_Keep.Services.ServiceTiposVeiculos.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Auto_Keep.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class TiposVeiculosController : ControllerBase
    {
        private readonly ITiposVeiculosRepository _tiposVeiculosRepository;

        public TiposVeiculosController(ITiposVeiculosRepository tiposVeiculosRepository)
        {
            _tiposVeiculosRepository = tiposVeiculosRepository;
        }

        /// <summary>
        /// Obter lista de todoos tipos de veiculos
        /// </summary>
        [Authorize(Roles = "Administrador,Cliente")]
        [HttpGet("ListarTipos")]
        public async Task<ActionResult<IEnumerable<TiposVeiculos>>> ListarTipos()
        {
            var response = await _tiposVeiculosRepository.GetVeiculos();
            return Ok(response);
        }

        /// <summary>
        /// Filtrar tipo de veiculo por id do registro
        /// </summary>
        /// <param name="id_TipoVeiculo">Id do tipo de veiculo</param>
        [Authorize(Roles = "Administrador,Cliente")]
        [HttpGet("FiltrarTiposVeiculosPorId/{id_TipoVeiculo}")]
        public async Task<ActionResult<TiposVeiculos>> FiltrarTiposVeiculosPorId(int id_TipoVeiculo)
        {
            var response = await _tiposVeiculosRepository.GetById(id_TipoVeiculo);
            return Ok(response);
        }

        /// <summary>
        /// Registrar tipos de veículos
        /// </summary>
        /// <param name="postTiposVeiculos"></param>
        [Authorize(Roles = "Administrador")]
        [HttpPost("RegistrarTiposVeiculos")]
        public async Task<ActionResult> RegistrarTiposVeiculos([FromBody] PostTiposVeiculos postTiposVeiculos)
        {
            await _tiposVeiculosRepository.PostTiposVeiculos(postTiposVeiculos);
            return Ok();
        }

        /// <summary>
        /// Editar tipos de veículos
        /// </summary>
        /// <param name="id_TipoVeiculo">Id do tipo de veículo</param>
        /// <param name="putTiposVeiculos"></param>
        [Authorize(Roles = "Administrador")]
        [HttpPut("EditarTiposVeiculos/{id_TipoVeiculo}")]
        public async Task<ActionResult<TiposVeiculos>> EditarTiposVeiculos(int id_TipoVeiculo, [FromBody] PutTiposVeiculos putTiposVeiculos)
        {
            var response = await _tiposVeiculosRepository.PutTiposVeiculos(id_TipoVeiculo, putTiposVeiculos);
            return Ok(response);
        }

        /// <summary>
        /// Remover tipos de veículos
        /// </summary>
        /// <param name="id_TipoVeiculo">Id do tipo de veículo</param>
        [Authorize(Roles = "Administrador")]
        [HttpDelete("RemoverTiposVeiculos/{id_TipoVeiculo}")]
        public async Task<ActionResult<int>> RemoverTiposVeiculos(int id_TipoVeiculo)
        {
            var response = await _tiposVeiculosRepository.DeleteTiposVeiculos(id_TipoVeiculo);
            return Ok(response);
        }
    }
}
