using AutoMapper;
using GeorgiaTechLibrary.DTOs;
using DataAccess.Models;

namespace GeorgiaTechLibrary.Automappers
{
    public class LibraryProfile : Profile
    {
        public LibraryProfile()
        {
            CreateMap<Library, LibraryDTO>().ReverseMap();
        }
    }
}
