using AutoMapper;
using GeorgiaTechLibrary.DTOs;
using DataAccess.Models;

namespace GeorgiaTechLibrary.Automappers
{
    public class DigitalItemProfile : Profile
    {
        public DigitalItemProfile() 
        { 
            CreateMap<DigitalItem, DigitalItemDTO>().ReverseMap();
        }
    }
}
