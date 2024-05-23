using AutoMapper;
using GeorgiaTechLibrary.DTOs;
using DataAccess.Models;

namespace GeorgiaTechLibrary.Automappers
{
    public class LoanProfile : Profile
    {
        public LoanProfile()
        {
            CreateMap<BookLoan, LoanDTO>()
                .ForMember(loanDto => loanDto.LoanBookInstance, opt => opt.MapFrom(loan => loan.BookInstance))
                .ReverseMap();
            CreateMap<DigitalItemLoan, LoanDTO>()
                .ForMember(loanDto => loanDto.LoanDigitalItem, opt => opt.MapFrom(loan => loan.DigitalItem))
                .ReverseMap();
        }
    }
}
