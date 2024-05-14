using AutoMapper;
using GeorgiaTechLibrary.DTOs;
using GeorgiaTechLibrary.Models;

namespace GeorgiaTechLibrary.Automappers
{
    public class LoanProfile : Profile
    {
        public LoanProfile()
        {
            CreateMap<Loan, LoanDTO>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()))
            .ReverseMap();
        }
    }
}
