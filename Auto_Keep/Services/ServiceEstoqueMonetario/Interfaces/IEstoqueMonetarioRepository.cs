using Auto_Keep.Models.AutoKeep;

namespace Auto_Keep.Services.ServiceEstoqueMonetario.Interfaces
{
    public interface IEstoqueMonetarioRepository
    {
        Task<IEnumerable<EstoqueMonetario>> GetEstoqueGeral();
        Task<bool> GetContemQtdCedulasMoedas(int qtd, decimal cedulaMoeda);
        Task<EstoqueMonetario> GetById(int idEstoque);
        Task PostEstoque(EstoqueMonetario estoqueMonetario);
        Task<EstoqueMonetario> PutEstoque(int idEstoque, EstoqueMonetario estoqueMonetario);
        Task<EstoqueMonetario> PutQtdEstoque(int idEstoque, int qtd);
        Task<bool> PutRangeQtdEstoque(IEnumerable<EstoqueMonetario> estoqueMonetario);
        Task<int> DeleteEstoque(int idEstoque);
    }
}
