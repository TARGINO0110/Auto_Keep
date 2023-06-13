using Auto_Keep.Helpers;
using Auto_Keep.Models.AutoKeep;
using Auto_Keep.Models.DbContextAutoKeep;
using Auto_Keep.Services.ServiceTiposVeiculos.Interfaces;
using Auto_Keep.Utils.Validations.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Auto_Keep.Services.ServiceTiposVeiculos
{
    public class TiposVeiculosRepository : ITiposVeiculosRepository
    {
        private readonly AutoKeepContext _dbContext;
        private readonly IValidationAtributes _validationAtributes;

        public TiposVeiculosRepository(AutoKeepContext dbContext, IValidationAtributes validationAtributes)
        {
            _dbContext = dbContext;
            _validationAtributes = validationAtributes;
        }

        public async Task<IEnumerable<TiposVeiculos>> GetVeiculos()
        {
            try
            {
                return await _dbContext.TiposVeiculos
                                       .AsNoTracking()
                                       .Include(p => p.Precos)
                                       .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new AppException(ex.Message, ex);
            }
        }

        private async Task<bool> GetExistTipoVeiculo(char siglaVeiculo)
        {
            try
            {
                return await _dbContext.TiposVeiculos.AnyAsync(item => item.Sigla_Veiculo == siglaVeiculo);
            }
            catch (Exception ex)
            {
                throw new AppException(ex.Message, ex);
            }
        }

        public async Task<TiposVeiculos> GetById(int id_TipoVeiculo)
        {
            try
            {
                return await _dbContext.TiposVeiculos.FirstOrDefaultAsync(item => item.Id_TipoVeiculo == id_TipoVeiculo);
            }
            catch (Exception ex)
            {
                throw new AppException(ex.Message, ex);
            }
        }

        public async Task PostTiposVeiculos(TiposVeiculos tiposVeiculos)
        {
            _validationAtributes.AtributesRequestTiposVeiculos(tiposVeiculos);
            if (await GetExistTipoVeiculo((char)tiposVeiculos.Sigla_Veiculo)) { throw new AppException("O Tipo de veículo ja possui cadastrado na base de dados!"); }

            try
            {
                await _dbContext.AddAsync(tiposVeiculos);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new AppException(ex.Message, ex);
            }
        }

        public async Task<TiposVeiculos> PutTiposVeiculos(int id_TipoVeiculo, TiposVeiculos tiposVeiculos)
        {
            var tipoVeiculoBase = await GetById(id_TipoVeiculo) ?? throw new AppException("Nenhum Tipo de veículo identificado na base de dados!");

            _dbContext.Attach(tipoVeiculoBase);

            foreach (PropertyInfo inf in tiposVeiculos.GetType().GetProperties())
            {
                if (inf.GetValue(tiposVeiculos) != null && inf.Name != "Id_TipoVeiculo" && inf.Name != "Tipo_Veiculo" && inf.Name != "Precos")
                {
                    _dbContext.Entry(tipoVeiculoBase).Property(inf.Name).CurrentValue = inf.GetValue(tiposVeiculos);
                }
            }

            try
            {
                _dbContext.TiposVeiculos.Update(tipoVeiculoBase);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new AppException(ex.Message, ex);
            }

            return tipoVeiculoBase;
        }

        public async Task<int> DeleteTiposVeiculos(int id_TipoVeiculo)
        {
            var tipoVeiculoBase = await GetById(id_TipoVeiculo) ?? throw new AppException("Nenhum Tipo de veículo identificado na base de dados!");

            try
            {
                _dbContext.Remove(tipoVeiculoBase);
                await _dbContext.SaveChangesAsync();

                return id_TipoVeiculo;
            }
            catch (Exception ex)
            {
                throw new AppException(ex.Message, ex);
            }
        }
    }
}
