using Auto_Keep.Models.AutoKeep;

namespace Auto_Keep.Services.ServiceHistoricoVeiculos.Interfaces
{
    public interface IHistoricoVeiculosRepository
    {
        Task<IEnumerable<HistoricoVeiculos>> GetHistoricosVeiculosGeral(int? page, int? rows);
        Task<IEnumerable<HistoricoVeiculos>> GetHistoricoVeiculosPlacas(string placaVeiculo, int? page, int? rows);
        Task<HistoricoVeiculos> GetById(long id_HistVeiculo);
        Task<bool> GetStatusVeiculo(HistoricoVeiculos historicoVeiculos);
        Task PostEntradaVeiculo(PostHistoricoVeiculos postHistoricoVeiculos);
        Task<HistoricoVeiculos> PutSaidaVeiculo(long id_HistVeiculo, PutHistoricoVeiculos putHistoricoVeiculos);
        Task<(decimal, TimeSpan)> GetObterValorEstadiaAtual(long id_HistVeiculo);
        Task<long> DeleteHistVeiculo(long id_HistVeiculo);
    }
}
