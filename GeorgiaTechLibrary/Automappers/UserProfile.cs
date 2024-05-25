using AutoMapper;
using GeorgiaTechLibrary.DTOs;
using DataAccess.Models;

namespace GeorgiaTechLibrary.Automappers
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDTO>()
                .ReverseMap();
            CreateMap<Member, MemberDTO>()
                .IncludeBase<User, UserDTO>()
                .ReverseMap();
            CreateMap<Staff, StaffDTO>()
                .IncludeBase<User, UserDTO>()
                .ReverseMap();
        }
    }
}
