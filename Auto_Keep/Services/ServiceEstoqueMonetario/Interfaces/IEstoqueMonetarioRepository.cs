using Auto_Keep.Models.AutoKeep;

namespace Auto_Keep.Services.ServiceEstoqueMonetario.Interfaces
{
    public interface IEstoqueMonetarioRepository
    {
        Task<IEnumerable<EstoqueMonetario>> GetEstoqueGeral();
        Task<bool> GetContemQtdCedulasMoedas(int qtd, decimal cedulaMoeda);
        Task<EstoqueMonetario> GetById(int idEstoque);
        Task PostEstoque(PostEstoqueMonetario postEstoqueMonetario);
        Task<EstoqueMonetario> PutEstoque(int idEstoque, PutEstoqueMonetario putEstoqueMonetario);
        Task<EstoqueMonetario> PutQtdEstoque(int idEstoque, int qtd);
        Task<bool> PutRangeQtdEstoque(IEnumerable<EstoqueMonetario> estoqueMonetario);
        Task<int> DeleteEstoque(int idEstoque);
    }
}
