using Auto_Keep.Models.AutoKeep;
using Auto_Keep.Services.ServiceHistoricoVeiculos.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auto_Keep.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class HistoricoVeiculosController : ControllerBase
    {
        private readonly IHistoricoVeiculosRepository _historicoVeiculosRepository;

        public HistoricoVeiculosController(IHistoricoVeiculosRepository historicoVeiculosRepository)
        {
            _historicoVeiculosRepository = historicoVeiculosRepository;
        }

        /// <summary>
        /// Obter Historico de todos veículos
        /// </summary>
        /// <param name="page">Apresentar por página</param>
        /// <param name="rows">Quantidade de linhas</param>
        [Authorize(Roles = "Administrador")]
        [HttpGet("ListarHistoricosVeiculosGeral")]
        public async Task<ActionResult<IEnumerable<HistoricoVeiculos>>> ListarHistoricosVeiculosGeral(int? page, int? rows)
        {
            var response = await _historicoVeiculosRepository.GetHistoricosVeiculosGeral(page, rows);
            return Ok(response);
        }

        /// <summary>
        /// Obter Historico do veículo pela placa
        /// </summary>
        /// <param name="placaVeiculo">Filtrar pela placa do veículo</param>
        /// <param name="page">Apresentar por página</param>
        /// <param name="rows">Quantidade de linhas</param>
        [Authorize(Roles = "Administrador,Cliente")]
        [HttpGet("ListarHistoricosVeiculosPlacas/{placaVeiculo}")]
        public async Task<ActionResult<IEnumerable<HistoricoVeiculos>>> ListarHistoricosVeiculosPlacas(string placaVeiculo, int? page, int? rows)
        {
            var response = await _historicoVeiculosRepository.GetHistoricoVeiculosPlacas(placaVeiculo, page, rows);
            return Ok(response);
        }

        /// <summary>
        /// Filtrar Historico do veículo pelo id do registro
        /// </summary>
        ///<param name="id_HistVeiculo">Id do veículo</param>
        [Authorize(Roles = "Administrador,Cliente")]
        [HttpGet("FiltrarRegistroId/{id_HistVeiculo}")]
        public async Task<ActionResult<HistoricoVeiculos>> FiltrarRegistroId(long id_HistVeiculo)
        {
            var response = await _historicoVeiculosRepository.GetById(id_HistVeiculo);
            return Ok(response);
        }

        /// <summary>
        /// Obter valor e duração da estadia atual em aberto
        /// </summary>
        /// <param name="id_HistVeiculo"></param>
        [Authorize(Roles = "Administrador,Cliente")]
        [HttpGet("ObterValorEstadiaAtual/{id_HistVeiculo}")]
        public async Task<ActionResult<HistoricoVeiculos>> ObterValorEstadiaAtual(long id_HistVeiculo)
        {
            var response = await _historicoVeiculosRepository.GetObterValorEstadiaAtual(id_HistVeiculo);
            return Ok(response);
        }

        /// <summary>
        /// Registrar entrada do veículo
        /// </summary>
        /// <param name="postHistoricoVeiculos"></param>
        [Authorize(Roles = "Administrador,Cliente")]
        [HttpPost("RegistrarHistoricoVeiculo")]
        public async Task<ActionResult> RegistrarHistoricoVeiculo([FromBody] PostHistoricoVeiculos postHistoricoVeiculos)
        {
            await _historicoVeiculosRepository.PostEntradaVeiculo(postHistoricoVeiculos);
            return Ok();
        }

        /// <summary>
        /// Atualizar saída do veículo
        /// </summary>
        /// <param name="id_HistoricoVeiculo">Id do registro</param>
        /// <param name="putHistoricoVeiculos"></param>
        [Authorize(Roles = "Administrador,Cliente")]
        [HttpPut("AtualizarSaidaVeiculo/{id_HistoricoVeiculo}")]
        public async Task<ActionResult<HistoricoVeiculos>> AtualizarSaidaVeiculo(long id_HistoricoVeiculo, [FromBody] PutHistoricoVeiculos putHistoricoVeiculos)
        {
            var response = await _historicoVeiculosRepository.PutSaidaVeiculo(id_HistoricoVeiculo, putHistoricoVeiculos);
            return Ok(response);
        }

        /// <summary>
        /// Remover histórico do veículo
        /// </summary>
        /// <param name="id_HistoricoVeiculo">Id do registro</param>
        [Authorize(Roles = "Administrador")]
        [HttpDelete("RemoverHistoricoVeiculo/{id_HistoricoVeiculo}")]
        public async Task<ActionResult<long>> RemoverHistoricoVeiculo(long id_HistoricoVeiculo)
        {
            long response = await _historicoVeiculosRepository.DeleteHistVeiculo(id_HistoricoVeiculo);
            return Ok(response);
        }

    }
}
