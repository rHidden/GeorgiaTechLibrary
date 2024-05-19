using AutoMapper;
using GeorgiaTechLibrary.DTOs;
using DataAccess.Models;

namespace GeorgiaTechLibrary.Automappers
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDTO>().ReverseMap();
        }
    }
}
