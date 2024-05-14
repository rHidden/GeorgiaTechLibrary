using AutoMapper;
using GeorgiaTechLibrary.DTOs;
using GeorgiaTechLibrary.Models;

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
