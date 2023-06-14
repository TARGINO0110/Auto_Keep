using Auto_Keep.Helpers;
using Auto_Keep.Models.AutoKeep;
using Auto_Keep.Models.DbContextAutoKeep;
using Auto_Keep.Services.ServicePrecos.Interfaces;
using Auto_Keep.Services.ServiceTiposVeiculos.Interfaces;
using Auto_Keep.Utils.Validations.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Auto_Keep.Services.ServicePrecos
{
    public class PrecosRepository : IPrecosRepository
    {
        private readonly AutoKeepContext _dbContext;
        private readonly IValidationAtributes _validationAtributes;
        private readonly ITiposVeiculosRepository _tiposVeiculosRepository;
        private readonly IMapper _mapper;

        public PrecosRepository(
            AutoKeepContext dbContext,
            IValidationAtributes validationAtributes,
            ITiposVeiculosRepository tiposVeiculosRepository,
            IMapper mapper
            )
        {
            _dbContext = dbContext;
            _validationAtributes = validationAtributes;
            _tiposVeiculosRepository = tiposVeiculosRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Precos>> GetPrecos()
        {
            try
            {
                return await _dbContext.Precos.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new AppException(ex.Message, ex);
            }
        }

        public async Task<Precos> GetPrecosTipoVeiculo(int id_TipoVeiculo)
        {
            try
            {
                return await _dbContext.Precos.FirstOrDefaultAsync(item => item.Id_TipoVeiculo == id_TipoVeiculo);
            }
            catch (Exception ex)
            {
                throw new AppException(ex.Message, ex);
            }
        }

        private async Task<bool> GetExistPrecosTipoVeiculo(int id_TipoVeiculo)
        {
            try
            {
                return await _dbContext.Precos.AnyAsync(item => item.Id_TipoVeiculo == id_TipoVeiculo);
            }
            catch (Exception ex)
            {
                throw new AppException(ex.Message, ex);
            }
        }

        public async Task<Precos> GetById(int id_Preco)
        {
            try
            {
                return await _dbContext.Precos.FirstOrDefaultAsync(item => item.Id_Preco == id_Preco);
            }
            catch (Exception ex)
            {
                throw new AppException(ex.Message, ex);
            }
        }

        public async Task PostPrecos(PostPrecos postPrecos)
        {
            var mapPostPrecos = _mapper.Map<Precos>(postPrecos);

            _validationAtributes.AtributesRequestPrecos(mapPostPrecos);
            if (await GetExistPrecosTipoVeiculo(mapPostPrecos.Id_TipoVeiculo)) { throw new AppException("O Tipo de veículo ja possui preço cadastrado!"); }
            if (await _tiposVeiculosRepository.GetById(mapPostPrecos.Id_TipoVeiculo) is null) { throw new AppException("O id do veículo não foi encontrado na base de dados!"); }

            try
            {
                await _dbContext.AddAsync(mapPostPrecos);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new AppException(ex.Message, ex);
            }
        }

        public async Task<Precos> PutPrecos(int idPrecos, PutPrecos putPrecos)
        {
            var mapPutPrecos = _mapper.Map<Precos>(putPrecos);

            var precosBase = await GetById(idPrecos) ?? throw new AppException("O preço não foi identificado na base de dados");
            if (await _tiposVeiculosRepository.GetById(mapPutPrecos.Id_TipoVeiculo) is null) { throw new AppException("O id do veículo não foi encontrado na base de dados!"); }

            _dbContext.Attach(precosBase);

            foreach (PropertyInfo inf in mapPutPrecos.GetType().GetProperties())
            {
                if (inf.GetValue(mapPutPrecos) != null && inf.Name != "Id_Preco" && inf.Name != "TiposVeiculos")
                {
                    _dbContext.Entry(precosBase).Property(inf.Name).CurrentValue = inf.GetValue(mapPutPrecos);
                }
            }

            try
            {
                _dbContext.Precos.Update(precosBase);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new AppException(ex.Message, ex);
            }

            return precosBase;
        }

        public async Task<int> DeletePrecos(int idPrecos)
        {
            var precoBase = await GetById(idPrecos) ?? throw new AppException("Nenhum preço identificado na base de dados!");

            try
            {
                _dbContext.Remove(precoBase);
                await _dbContext.SaveChangesAsync();

                return idPrecos;
            }
            catch (Exception ex)
            {
                throw new AppException(ex.Message, ex);
            }
        }
    }
}
