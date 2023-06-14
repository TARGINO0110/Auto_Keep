using Auto_Keep.Models.AutoKeep;
using AutoMapper;

namespace Auto_Keep.Mappings
{
    public class MapperPrecos : Profile
    {
        public MapperPrecos()
        {
            CreateMap<Precos, PostPrecos>().ReverseMap();
            CreateMap<Precos, PutPrecos>().ReverseMap();
        }
    }
}
