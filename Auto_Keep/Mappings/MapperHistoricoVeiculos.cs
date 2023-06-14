using Auto_Keep.Models.AutoKeep;
using AutoMapper;

namespace Auto_Keep.Mappings
{
    public class MapperHistoricoVeiculos : Profile
    {
        public MapperHistoricoVeiculos()
        {
            CreateMap<HistoricoVeiculos, PostHistoricoVeiculos>().ReverseMap();
            CreateMap<HistoricoVeiculos, PutHistoricoVeiculos>().ReverseMap();
        }
    }
}
