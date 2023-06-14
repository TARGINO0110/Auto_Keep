using Auto_Keep.Helpers;
using Auto_Keep.Models.AutoKeep;
using Auto_Keep.Models.DbContextAutoKeep;
using Auto_Keep.Services.ServiceEstoqueMonetario.Interfaces;
using Auto_Keep.Utils.Validations.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Auto_Keep.Services.ServiceEstoqueMonetario
{
    public class EstoqueMonetarioRepository : IEstoqueMonetarioRepository
    {
        private readonly AutoKeepContext _dbContext;
        private readonly IValidationAtributes _validationAtributes;
        public EstoqueMonetarioRepository(AutoKeepContext dbContext, IValidationAtributes validationAtributes)
        {
            _dbContext = dbContext;
            _validationAtributes = validationAtributes;
        }

        public async Task<IEnumerable<EstoqueMonetario>> GetEstoqueGeral()
        {
            try
            {
                return await _dbContext.EstoqueMonetarios.AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                throw new AppException(ex.Message, ex);
            }
        }

        private async Task<bool> GetValidaCedulaMoedaExiste(decimal valorNotaMoeda)
        {
            try
            {
                return await _dbContext.EstoqueMonetarios.AnyAsync(item => item.Valor_Nota_Moeda == valorNotaMoeda);
            }
            catch (Exception ex)
            {
                throw new AppException(ex.Message, ex);
            }
        }

        public async Task<bool> GetContemQtdCedulasMoedas(int qtd, decimal cedulaMoeda)
        {
            try
            {
                return await _dbContext.EstoqueMonetarios.AnyAsync(item => item.Qtd >= qtd && item.Valor_Nota_Moeda == cedulaMoeda);
            }
            catch (Exception ex)
            {
                throw new AppException(ex.Message, ex);
            }
        }

        public async Task<EstoqueMonetario> GetById(int idEstoque)
        {
            try
            {
                return await _dbContext.EstoqueMonetarios.FirstOrDefaultAsync(item => item.Id_Estoque == idEstoque);
            }
            catch (Exception ex)
            {
                throw new AppException(ex.Message, ex);
            }
        }

        public async Task PostEstoque(EstoqueMonetario estoqueMonetario)
        {
            //Validar Atributos para Post ou Put
            _validationAtributes.AtributesRequestEstoqueMonetario(estoqueMonetario);
            if (await GetValidaCedulaMoedaExiste((decimal)estoqueMonetario.Valor_Nota_Moeda)) throw new AppException("A nota ou moeda já existe em nossa base de dados!");

            estoqueMonetario.Dt_Atualizado = DateTime.Now.ToUniversalTime();

            try
            {
                await _dbContext.AddAsync(estoqueMonetario);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new AppException(ex.Message, ex);
            }
        }

        public async Task<EstoqueMonetario> PutEstoque(int idEstoque, EstoqueMonetario estoqueMonetario)
        {
            //Validar Atributos para Post ou Put
            _validationAtributes.AtributesRequestEstoqueMonetario(estoqueMonetario);
            var estoqueMonetarioBase = await GetById(idEstoque) ?? throw new AppException("Nenhum estoque identificado na base de dados!");

            estoqueMonetarioBase.Dt_Atualizado = DateTime.Now.ToUniversalTime();

            _dbContext.Attach(estoqueMonetarioBase);

            foreach (PropertyInfo inf in estoqueMonetario.GetType().GetProperties())
            {
                if (inf.GetValue(estoqueMonetario) != null && inf.Name != "Id_Estoque")
                {
                    _dbContext.Entry(estoqueMonetarioBase).Property(inf.Name).CurrentValue = inf.GetValue(estoqueMonetario);
                }
            }

            try
            {
                _dbContext.Update(estoqueMonetarioBase);
                await _dbContext.SaveChangesAsync();

                return estoqueMonetarioBase;
            }
            catch (Exception ex)
            {
                throw new AppException(ex.Message, ex);
            }
        }

        public async Task<EstoqueMonetario> PutQtdEstoque(int idEstoque, int qtd)
        {
            //Validar Atributos para Post ou Put
            _validationAtributes.AtributesRequestPutQtdEstoqueMonetario(qtd);
            var estoqueBase = await GetById(idEstoque) ?? throw new AppException("Nenhum estoque identificado na base de dados!");

            estoqueBase.Qtd = qtd;

            try
            {
                _dbContext.Attach(estoqueBase);
                _dbContext.Update(estoqueBase);
                await _dbContext.SaveChangesAsync();

                return estoqueBase;
            }
            catch (Exception ex)
            {
                throw new AppException(ex.Message, ex);
            }
        }

        public async Task<bool> PutRangeQtdEstoque(IEnumerable<EstoqueMonetario> estoqueMonetario)
        {
            try
            {
                _dbContext.AttachRange(estoqueMonetario);
                _dbContext.UpdateRange(estoqueMonetario);
                await _dbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new AppException(ex.Message, ex);
            }
        }

        public async Task<int> DeleteEstoque(int idEstoque)
        {
            var estoqueBase = await GetById(idEstoque) ?? throw new AppException("Nenhum estoque identificado na base de dados!");
            try
            {
                _dbContext.Remove(estoqueBase);
                await _dbContext.SaveChangesAsync();

                return idEstoque;
            }
            catch (Exception ex)
            {
                throw new AppException(ex.Message, ex);
            }
        }
    }
}
