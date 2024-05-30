using AutoMapper;
using GeorgiaTechLibrary.DTOs;
using DataAccess.Models;

namespace GeorgiaTechLibrary.Automappers
{
    public class LateUserProfile : Profile
    {
        public LateUserProfile()
        {
            CreateMap<(User User, int SumOfDaysOfBeingLate), LateUserDto>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User))
                .ForMember(dest => dest.SumOfDaysOfBeingLate, opt => opt.MapFrom(src => src.SumOfDaysOfBeingLate));
        }
    }
}
