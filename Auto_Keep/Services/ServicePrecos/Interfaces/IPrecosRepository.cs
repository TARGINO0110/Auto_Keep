using Auto_Keep.Models.AutoKeep;

namespace Auto_Keep.Services.ServicePrecos.Interfaces
{
    public interface IPrecosRepository
    {
        Task<IEnumerable<Precos>> GetPrecos();
        Task<Precos> GetPrecosTipoVeiculo(int id_TipoVeiculo);
        Task<Precos> GetById(int id_Preco);
        Task PostPrecos(Precos precos);
        Task<Precos> PutPrecos(int idPrecos, Precos precos);
        Task<int> DeletePrecos(int idPrecos);
    }
}
