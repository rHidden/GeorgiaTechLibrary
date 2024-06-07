using AutoMapper;
using Webshop.User.Application.Features.Dto;
using Webshop.User.Application.Features.Requests;

namespace Webshop.User.Application.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Domain.AggregateRoots.User, UserDto>().ReverseMap();     
            CreateMap<Domain.AggregateRoots.User, CreateUserRequest>().ReverseMap();
            CreateMap<Domain.AggregateRoots.User, UpdateUserRequest>().ReverseMap();
        }
    }
}
