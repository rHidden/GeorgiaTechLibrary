using AutoMapper;
using GeorgiaTechLibrary.DTOs;
using DataAccess.Models;

namespace GeorgiaTechLibrary.Automappers
{
    public class AverageLoanDurationProfile : Profile
    {
        public AverageLoanDurationProfile()
        {
            CreateMap<(User User, int AvgLoanDuration), AverageLoanDurationDTO>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User))
                .ForMember(dest => dest.AverageLoanDuration, opt => opt.MapFrom(src => src.AvgLoanDuration));
        }
    }
}
