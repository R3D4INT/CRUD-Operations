using AutoMapper;
using backend.Dtos.Request;

namespace backend.DAL
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserRequest>().ReverseMap();
        }
    }
}
