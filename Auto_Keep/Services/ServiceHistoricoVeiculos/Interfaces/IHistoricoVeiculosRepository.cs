using Auto_Keep.Models.AutoKeep;

namespace Auto_Keep.Services.ServiceHistoricoVeiculos.Interfaces
{
    public interface IHistoricoVeiculosRepository
    {
        Task<IEnumerable<HistoricoVeiculos>> GetHistoricosVeiculosGeral(int? page, int? rows);
        Task<IEnumerable<HistoricoVeiculos>> GetHistoricoVeiculosPlacas(string placaVeiculo, int? page, int? rows);
        Task<HistoricoVeiculos> GetById(long id_HistVeiculo);
        Task<bool> GetStatusVeiculo(HistoricoVeiculos historicoVeiculos);
        Task PostEntradaVeiculo(HistoricoVeiculos historicoVeiculos);
        Task<HistoricoVeiculos> PutSaidaVeiculo(long id_HistVeiculo, HistoricoVeiculos historicoVeiculos);
        Task<long> DeleteHistVeiculo(long id_HistVeiculo);
    }
}
