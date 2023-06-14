using Auto_Keep.Models.AutoKeep;

namespace Auto_Keep.Services.ServiceTiposVeiculos.Interfaces
{
    public interface ITiposVeiculosRepository
    {
        Task<IEnumerable<TiposVeiculos>> GetVeiculos();
        Task<TiposVeiculos> GetById(int id_TipoVeiculo);
        Task PostTiposVeiculos(PostTiposVeiculos postTiposVeiculos);
        Task<TiposVeiculos> PutTiposVeiculos(int id_TipoVeiculo, PutTiposVeiculos putTiposVeiculos);
        Task<int> DeleteTiposVeiculos(int id_TipoVeiculo);
    }
}
