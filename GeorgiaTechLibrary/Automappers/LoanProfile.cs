using AutoMapper;
using GeorgiaTechLibrary.DTOs;
using DataAccess.Models;

namespace GeorgiaTechLibrary.Automappers
{
    public class LoanProfile : Profile
    {
        public LoanProfile()
        {
            CreateMap<Loan, LoanDTO>().ReverseMap();
        }
    }
}
