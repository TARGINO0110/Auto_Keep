using Auto_Keep.Models.AutoKeep;
using AutoMapper;

namespace Auto_Keep.Mappings
{
    public class MapperTiposVeiculos : Profile
    {
        public MapperTiposVeiculos()
        {
            CreateMap<TiposVeiculos, PostTiposVeiculos>().ReverseMap();
            CreateMap<TiposVeiculos, PutTiposVeiculos>().ReverseMap();
        }
    }
}
