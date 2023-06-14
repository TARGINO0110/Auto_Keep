using Auto_Keep.Helpers;
using Auto_Keep.Models.AutoKeep;
using Auto_Keep.Models.DbContextAutoKeep;
using Auto_Keep.Services.ServiceEstoqueMonetario.Interfaces;
using Auto_Keep.Utils.Validations.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Auto_Keep.Services.ServiceEstoqueMonetario
{
    public class EstoqueMonetarioRepository : IEstoqueMonetarioRepository
    {
        private readonly AutoKeepContext _dbContext;
        private readonly IValidationAtributes _validationAtributes;
        private readonly IMapper _mapper;

        public EstoqueMonetarioRepository(
            AutoKeepContext dbContext,
            IValidationAtributes validationAtributes,
            IMapper mapper)
        {
            _dbContext = dbContext;
            _validationAtributes = validationAtributes;
            _mapper = mapper;
        }

        public async Task<IEnumerable<EstoqueMonetario>> GetEstoqueGeral()
        {
            try
            {
                return await _dbContext.EstoqueMonetarios.AsNoTracking().OrderBy(item => item.Valor_Nota_Moeda).ToListAsync();
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

        public async Task PostEstoque(PostEstoqueMonetario postEstoqueMonetario)
        {
            var mapPostEstoqueMonetario = _mapper.Map<EstoqueMonetario>(postEstoqueMonetario);

            //Validar Atributos para Post ou Put
            _validationAtributes.AtributesRequestEstoqueMonetario(mapPostEstoqueMonetario);
            if (await GetValidaCedulaMoedaExiste((decimal)mapPostEstoqueMonetario.Valor_Nota_Moeda)) throw new AppException("A nota ou moeda já existe em nossa base de dados!");

            mapPostEstoqueMonetario.Dt_Atualizado = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);

            try
            {
                await _dbContext.AddAsync(mapPostEstoqueMonetario);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new AppException(ex.Message, ex);
            }
        }

        public async Task<EstoqueMonetario> PutEstoque(int idEstoque, PutEstoqueMonetario putEstoqueMonetario)
        {
            var mapPutEstoqueMonetario = _mapper.Map<EstoqueMonetario>(putEstoqueMonetario);

            //Validar Atributos para Post ou Put
            _validationAtributes.AtributesRequestEstoqueMonetario(mapPutEstoqueMonetario);
            var estoqueMonetarioBase = await GetById(idEstoque) ?? throw new AppException("Nenhum estoque identificado na base de dados!");

            mapPutEstoqueMonetario.Dt_Atualizado = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);

            _dbContext.Attach(estoqueMonetarioBase);

            foreach (PropertyInfo inf in mapPutEstoqueMonetario.GetType().GetProperties())
            {
                if (inf.GetValue(mapPutEstoqueMonetario) != null && inf.Name != "Id_Estoque")
                {
                    _dbContext.Entry(estoqueMonetarioBase).Property(inf.Name).CurrentValue = inf.GetValue(mapPutEstoqueMonetario);
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
