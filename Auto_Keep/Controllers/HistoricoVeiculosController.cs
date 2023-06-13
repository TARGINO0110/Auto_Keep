using Auto_Keep.Models.AutoKeep;
using Auto_Keep.Services.ServiceHistoricoVeiculos.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Auto_Keep.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/[controller]")]
    [ApiController]
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
        [HttpGet("ListarHistoricosVeiculosPlacas")]
        public async Task<ActionResult<IEnumerable<HistoricoVeiculos>>> ListarHistoricosVeiculosPlacas(string placaVeiculo, int? page, int? rows)
        {
            var response = await _historicoVeiculosRepository.GetHistoricoVeiculosPlacas(placaVeiculo, page, rows);
            return Ok(response);
        }

        /// <summary>
        /// Filtrar Historico do veículo pelo id do registro
        /// </summary>
        ///<param name="id_HistVeiculo">Id do veículo</param>
        [HttpGet("FiltrarRegistroId")]
        public async Task<ActionResult<HistoricoVeiculos>> FiltrarRegistroId(long id_HistVeiculo)
        {
            var response = await _historicoVeiculosRepository.GetById(id_HistVeiculo);
            return Ok(response);
        }
    }
}
