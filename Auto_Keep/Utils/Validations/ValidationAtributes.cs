using Auto_Keep.Helpers;
using Auto_Keep.Models.AutoKeep;
using Auto_Keep.Utils.Validations.Interfaces;
using System.Linq;

namespace Auto_Keep.Utils.Validations
{
    public class ValidationAtributes : IValidationAtributes
    {
        #region Estoque Monetario

        public void AtributesRequestEstoqueMonetario(EstoqueMonetario estoqueMonetario)
        {
            if (estoqueMonetario.Valor_Nota_Moeda is null)
                throw new AppException("Necessário informar o valor da nota ou moeda!");
            if (!CedulaMoedaValida((decimal)estoqueMonetario.Valor_Nota_Moeda))
                throw new AppException($"A nota ou moeda não existe no padrão Real, informe as Seguintes opções: {string.Join("\n", NotasMoedas().ToList())}");
            if (estoqueMonetario.Qtd is null)
                throw new AppException("Informar a quantidade para esse tipo de valor!");
        }

        public void AtributesRequestPutQtdEstoqueMonetario(int qtd)
        {
            if (qtd == 0) throw new AppException("Necessário informar quantidade de cedula ou moeda maior que 0");
        }

        #endregion

        #region Preços

        public void AtributesRequestPrecos(Precos precos)
        {
            if (precos.Preco is null) throw new AppException("Necessácio informar o preço do veículo!");
            if (precos.Id_TipoVeiculo == 0) throw new AppException("Necessário informar o código do veículo para identificação no preço!");
        }

        #endregion

        #region Tipos de Veiculos

        public void AtributesRequestTiposVeiculos(TiposVeiculos tiposVeiculos)
        {
            if (tiposVeiculos.Sigla_Veiculo is null || !SiglasVeiculos().Any(item => item[0].Equals(tiposVeiculos.Sigla_Veiculo))) throw new AppException($"Necessario informar a sigla do tipo de veículo para identificação, opções disponiveis: {string.Join("\n", SiglasVeiculos().ToList())}");
        }

        private static List<string> SiglasVeiculos()
        {
            return new List<string> { "M - Moto", "C - Carro", "O - Ônibus" };
        }

        #endregion

        #region Histórico de Veículos
        public void AtributesRequestHistoricoEntradaVeiculos(HistoricoVeiculos historicoEntradaVeiculos)
        {
            if (string.IsNullOrEmpty(historicoEntradaVeiculos.PlacaVeiculo)) throw new AppException("Informe a placa do seu veículo para processo de identificação!");
            if (historicoEntradaVeiculos.Id_TiposVeiculos is null) throw new AppException("Necessário informar o tipo do seu veículo para contabilizar o preço padrão!");
            //VALIDAR PLACA DO VEÍCULO
            if (historicoEntradaVeiculos.PlacaVeiculo.Length != 7) throw new AppException("A sua placa do veículo se encontra incoerente com o padrão definido pela DETRAN!");
        }

        public void AtributesRequestHistoricoSaidaVeiculos(HistoricoVeiculos historicoSaidaVeiculos)
        {
            if (historicoSaidaVeiculos.Notas_Moedas is null) throw new AppException("Necessário informar suas notas e moedas para pagamento da estadia do estacionamento!");
            AtributesRequestHistoricoEntradaVeiculos(historicoSaidaVeiculos);
        }

        public void ValidarNotasMoedas(List<decimal?> cedulasMoedas)
        {
            foreach (decimal cm in cedulasMoedas)
                if (!CedulaMoedaValida(cm))
                    throw new AppException($"Erro nesse valor {cm} As notas e moedas precisam ser passadas separas, segue o exemplo: {string.Join("\n", NotasMoedas().ToList())}");
        }

        #endregion

        protected bool CedulaMoedaValida(decimal cedulaMoeda)
        {
            return (cedulaMoeda)
            switch
            {
                (0.05m) => true,
                (0.10m) => true,
                (0.25m) => true,
                (0.50m) => true,
                (1.00m) => true,
                (2.00m) => true,
                (5.00m) => true,
                (10.00m) => true,
                (20.00m) => true,
                (50.00m) => true,
                (100.00m) => true,
                (200.00m) => true,
                _ => false,
            };
        }

        private static List<string> NotasMoedas()
        {
            return new List<string> { "0.05", "0.10", "0.25", "0.50", "1.00", "2.00", "5.00", "10.00", "20.00", "50.00", "100.00", "200.00" };
        }
    }
}
