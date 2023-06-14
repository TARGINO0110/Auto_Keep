using Auto_Keep.Models.AutoKeep;
using Auto_Keep.Services.ServicePrecos.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auto_Keep.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class PrecosController : ControllerBase
    {
        private readonly IPrecosRepository _precosRepository;

        public PrecosController(IPrecosRepository precosRepository)
        {
            _precosRepository = precosRepository;
        }

        /// <summary>
        /// Obter lista de preços de todos veículos
        /// </summary>
        [Authorize(Roles = "Administrador,Cliente")]
        [HttpGet("ListarPrecos")]
        public async Task<ActionResult<IEnumerable<Precos>>> ListarPrecos()
        {
            var response = await _precosRepository.GetPrecos();
            return Ok(response);
        }

        /// <summary>
        /// Filtrar preços por tipo de veículos
        /// </summary>
        /// <param name="id_Veiculos">Id do veículo</param>
        [Authorize(Roles = "Administrador,Cliente")]
        [HttpGet("ListarPrecosTipoVeiculos/{id_Veiculos}")]
        public async Task<ActionResult<Precos>> ListarPrecosTipoVeiculos(int id_Veiculos)
        {
            var response = await _precosRepository.GetPrecosTipoVeiculo(id_Veiculos);
            return Ok(response);

        }

        /// <summary>
        /// Filtrar preço por id do registro
        /// </summary>
        /// <param name="id_Preco">Id do preço</param>
        [Authorize(Roles = "Administrador,Cliente")]
        [HttpGet("FiltrarPrecoPorId/{id_Preco}")]
        public async Task<ActionResult<Precos>> FiltrarPrecoPorId(int id_Preco)
        {
            var response = await _precosRepository.GetById(id_Preco);
            return Ok(response);
        }

        /// <summary>
        /// Registrar preços dos veículos
        /// </summary>
        /// <param name="postPrecos"></param>
        [Authorize(Roles = "Administrador")]
        [HttpPost("RegistrarPrecos")]
        public async Task<ActionResult> RegistrarPrecos([FromBody] PostPrecos postPrecos)
        {
            await _precosRepository.PostPrecos(postPrecos);
            return Ok();
        }

        /// <summary>
        /// Editar precos dos veículos
        /// </summary>
        /// <param name="id_Preco">Id do preço</param>
        /// <param name="putPrecos"></param>
        [Authorize(Roles = "Administrador")]
        [HttpPut("EditarPrecos/{id_Preco}")]
        public async Task<ActionResult<Precos>> EditarPrecos(int id_Preco, [FromBody] PutPrecos putPrecos)
        {
            var response = await _precosRepository.PutPrecos(id_Preco, putPrecos);
            return Ok(response);
        }

        /// <summary>
        /// Remover preços registrados
        /// </summary>
        /// <param name="id_Preco">Id do preço</param>
        [Authorize(Roles = "Administrador")]
        [HttpDelete("RemoverPrecos/{id_Preco}")]
        public async Task<ActionResult<int>> RemoverPrecos(int id_Preco)
        {
            var response = await _precosRepository.DeletePrecos(id_Preco);
            return Ok(response);
        }
    }
}
