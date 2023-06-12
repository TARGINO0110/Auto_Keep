using Auto_Keep.Models.AutoKeep;

namespace Auto_Keep.Utils.Validations.Interfaces
{
    public interface IValidationAtributes
    {
        void AtributesRequestEstoqueMonetario(EstoqueMonetario estoqueMonetario);
        void AtributesRequestPutQtdEstoqueMonetario(int qtd);
        void AtributesRequestPrecos(Precos precos);
        void AtributesRequestTiposVeiculos(TiposVeiculos tiposVeiculos);
        void AtributesRequestHistoricoEntradaVeiculos(HistoricoVeiculos historicoEntradaVeiculos);
        void AtributesRequestHistoricoSaidaVeiculos(HistoricoVeiculos historicoSaidaVeiculos);
        void ValidarNotasMoedas(List<decimal?> cedulasMoedas);
    }
}