using Auto_Keep.Helpers;
using Auto_Keep.Models.AutoKeep;
using Auto_Keep.Models.DbContextAutoKeep;
using Auto_Keep.Services.ServiceEstoqueMonetario.Interfaces;
using Auto_Keep.Services.ServiceHistoricoVeiculos.Interfaces;
using Auto_Keep.Utils.Paginated;
using Auto_Keep.Utils.Validations.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Auto_Keep.Services.ServiceHistoricoVeiculos
{
    public class HistoricoVeiculosRepository : IHistoricoVeiculosRepository
    {
        private readonly AutoKeepContext _dbContext;
        private readonly IValidationAtributes _validationAtributes;
        private readonly IEstoqueMonetarioRepository _estoqueMonetarioRepository;

        public HistoricoVeiculosRepository(AutoKeepContext dbContext, IValidationAtributes validationAtributes, IEstoqueMonetarioRepository estoqueMonetarioRepository)
        {
            _dbContext = dbContext;
            _validationAtributes = validationAtributes;
            _estoqueMonetarioRepository = estoqueMonetarioRepository;
        }

        public async Task<IEnumerable<HistoricoVeiculos>> GetHistoricosVeiculosGeral(int? page, int? rows)
        {
            try
            {
                IQueryable<HistoricoVeiculos> sourceData = null;
                sourceData = _dbContext.HistoricoVeiculos.AsNoTracking();

                return await PaginatedList<HistoricoVeiculos>.CreateAsync(sourceData, page ?? 1, rows ?? 200);
            }
            catch (Exception ex)
            {
                throw new AppException(ex.Message, ex);
            }
        }

        public async Task<IEnumerable<HistoricoVeiculos>> GetHistoricoVeiculosPlacas(string placaVeiculo, int? page, int? rows)
        {
            try
            {
                IQueryable<HistoricoVeiculos> sourceData = null;
                sourceData = _dbContext.HistoricoVeiculos.AsNoTracking().Where(item => item.PlacaVeiculo == placaVeiculo);

                return await PaginatedList<HistoricoVeiculos>.CreateAsync(sourceData, page ?? 1, rows ?? 200);
            }
            catch (Exception ex)
            {
                throw new AppException(ex.Message, ex);
            }
        }

        public async Task<HistoricoVeiculos> GetById(long id_HistVeiculo)
        {
            try
            {
                return await _dbContext.HistoricoVeiculos.FirstOrDefaultAsync(item => item.Id_HistVeiculo == id_HistVeiculo);
            }
            catch (Exception ex)
            {
                throw new AppException(ex.Message, ex);
            }
        }

        public async Task<bool> GetStatusVeiculo(HistoricoVeiculos historicoVeiculos)
        {
            try
            {
                return await _dbContext.HistoricoVeiculos.AnyAsync(item =>
                item.PlacaVeiculo == historicoVeiculos.PlacaVeiculo
                && historicoVeiculos.DataHora_Saida.HasValue
                && historicoVeiculos.Pago.Value
                );
            }
            catch (Exception ex)
            {
                throw new AppException(ex.Message, ex);
            }
        }

        public async Task PostEntradaVeiculo(HistoricoVeiculos historicoVeiculos)
        {
            _validationAtributes.AtributesRequestHistoricoEntradaVeiculos(historicoVeiculos);
            if (!await GetStatusVeiculo(historicoVeiculos)) throw new AppException("O veiculo ainda não deu baixa de sua estadia no estacionaento!");

            historicoVeiculos.DataHora_Saida = DateTime.Now;

            try
            {
                await _dbContext.AddAsync(historicoVeiculos);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new AppException(ex.Message, ex);
            }
        }

        public async Task<HistoricoVeiculos> PutSaidaVeiculo(long id_HistVeiculo, HistoricoVeiculos historicoVeiculos)
        {
            _validationAtributes.AtributesRequestHistoricoSaidaVeiculos(historicoVeiculos);
            if (await GetStatusVeiculo(historicoVeiculos)) throw new AppException("O veiculo já realizou baixa de sua estadia no estacionaento!");

            HistoricoVeiculos historicoVeiculosBase = await PrepareValoresPagamento(id_HistVeiculo, historicoVeiculos);

            _dbContext.Attach(historicoVeiculosBase);

            foreach (PropertyInfo inf in historicoVeiculos.GetType().GetProperties())
            {
                if (inf.GetValue(historicoVeiculos) != null && inf.Name != "Id_HistVeiculo" && inf.Name != "TiposVeiculos" && inf.Name != "Id_TiposVeiculos")
                {
                    _dbContext.Entry(historicoVeiculosBase).Property(inf.Name).CurrentValue = inf.GetValue(historicoVeiculos);
                }
            }

            try
            {
                _dbContext.Update(historicoVeiculosBase);
                await _dbContext.SaveChangesAsync();

                return historicoVeiculosBase;
            }
            catch (Exception ex)
            {
                throw new AppException(ex.Message, ex);
            }
        }

        private async Task<HistoricoVeiculos> PrepareValoresPagamento(long id_HistVeiculo, HistoricoVeiculos historicoVeiculos)
        {
            _validationAtributes.ValidarNotasMoedas(historicoVeiculos.Notas_Moedas);
            var valuesBase = await GetById(id_HistVeiculo) ?? throw new AppException("Nenhum Histórico de veículo identificado na base de dados!");

            //VALOR PAGO EM NOTAS E MOEDAS
            valuesBase.Valor_Pago = historicoVeiculos.Notas_Moedas.Sum();
            //CALCULAR PREÇO ESTADIA
            valuesBase.Valor_Total = decimal.Ceiling((decimal)(valuesBase.TiposVeiculos.Precos.Preco * valuesBase.Duracao));
            //CALCULO DO TROCO
            valuesBase.Valor_Troco = decimal.Subtract((decimal)historicoVeiculos.Valor_Pago, (decimal)historicoVeiculos.Valor_Total);

            var listCedulasMoedasTroco = await TotalizadorCedulasMoedas(decimal.ToDouble((decimal)historicoVeiculos.Valor_Pago), decimal.ToDouble((decimal)historicoVeiculos.Valor_Total));

            //DIMINUIR QTD CEDULAS E MOEDAS
            IEnumerable<EstoqueMonetario> estoqueBase = await _estoqueMonetarioRepository.GetEstoqueGeral();
            foreach (object cm in listCedulasMoedasTroco)
            {
                PropertyInfo cedulaMoeda = cm.GetType().GetProperties()[0];
                PropertyInfo qtd = cm.GetType().GetProperties()[1];
                decimal _cedulaMoeda = decimal.Parse(cedulaMoeda.GetValue(cm).ToString());
                int _qtd = int.Parse(qtd.GetValue(cm).ToString());

                estoqueBase.FirstOrDefault(item => item.Valor_Nota_Moeda == _cedulaMoeda).Qtd -= _qtd;
            }
            //ATUALIZAR QUANTIDADES NO ESTOQUE
            if (await _estoqueMonetarioRepository.PutRangeQtdEstoque(estoqueBase))
            {
                valuesBase.Pago = true;
                valuesBase.DataHora_Saida = DateTime.Now;
            }

            return valuesBase;
        }

        private async Task<List<object>> TotalizadorCedulasMoedas(double valorPago, double valorTotal)
        {
            List<object> listCedulasMoedasTroco = new();
            float[] moedas = { 200, 100, 50, 20, 10, 5, 2, 1, 0.50f, 0.25f, 0.10f, 0.05f };

            var troco = Math.Round(valorPago - valorTotal, 2);
            foreach (var moeda in moedas)
            {
                var qtd = (int)(troco / moeda);
                if (qtd > 0)
                {
                    troco -= Math.Round(qtd * moeda, 2);
                    decimal moedaDecimal = decimal.Parse(moeda.ToString("N2"));
                    //Verificar no estoque se possui Qtd de Cedulas e Moedas disponíveis
                    if (!await _estoqueMonetarioRepository.GetContemQtdCedulasMoedas(qtd, moedaDecimal))
                        throw new AppException($"Problemas com a quantidade de Cedulas e moedas de estoque para devolver {qtd}x de {moedaDecimal}, solicitar o Administrador para abastecer o estoque!");

                    listCedulasMoedasTroco.Add(new { CedulaMoeda = moedaDecimal, Qtd = qtd });
                }
            }

            return listCedulasMoedasTroco;
        }

        public async Task<long> DeleteHistVeiculo(long id_HistVeiculo)
        {
            var histVeicBase = await GetById(id_HistVeiculo) ?? throw new AppException("Nenhum Histórico de veículo identificado na base de dados!");

            try
            {
                _dbContext.Remove(histVeicBase);
                await _dbContext.SaveChangesAsync();

                return id_HistVeiculo;
            }
            catch (Exception ex)
            {
                throw new AppException(ex.Message, ex);
            }
        }
    }
}
