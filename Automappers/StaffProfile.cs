using AutoMapper;
using GeorgiaTechLibrary.DTOs;
using GeorgiaTechLibrary.Models;

namespace GeorgiaTechLibrary.Automappers
{
    public class StaffProfile : Profile
    {
        public StaffProfile()
        {
            CreateMap<Staff, StaffDTO>().ReverseMap();
        }
    }
}
