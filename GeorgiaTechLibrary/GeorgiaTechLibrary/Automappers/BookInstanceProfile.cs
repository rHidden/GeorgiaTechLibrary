using AutoMapper;
using GeorgiaTechLibrary.DTOs;
using DataAccess.Models;

namespace GeorgiaTechLibrary.Automappers
{
    public class BookInstanceProfile : Profile
    {
        public BookInstanceProfile()
        {
            CreateMap<BookInstance, BookInstanceDTO>().ReverseMap();
        }
    }
}
