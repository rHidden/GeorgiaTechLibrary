using AutoMapper;
using GeorgiaTechLibrary.DTOs;
using DataAccess.Models;

namespace GeorgiaTechLibrary.Automappers
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            CreateMap<Book, BookDTO>().ReverseMap();
        }
    }
}
