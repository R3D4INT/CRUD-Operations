using AutoMapper;
using backend.Dtos.Request;
using backend.Models;

namespace backend.DAL
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserRequest>().ReverseMap();
            CreateMap<Country, CountryRequest>().ReverseMap();
        }
    }
}
