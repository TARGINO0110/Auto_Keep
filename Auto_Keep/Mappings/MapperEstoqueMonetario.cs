using Auto_Keep.Models.AutoKeep;
using AutoMapper;

namespace Auto_Keep.Mappings
{
    public class MapperEstoqueMonetario: Profile
    {
        public MapperEstoqueMonetario()
        {
            CreateMap<EstoqueMonetario, PostEstoqueMonetario>().ReverseMap();
            CreateMap<EstoqueMonetario, PutEstoqueMonetario>().ReverseMap();
        }
    }
}
