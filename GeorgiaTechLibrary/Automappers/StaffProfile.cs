using AutoMapper;
using GeorgiaTechLibrary.DTOs;
using DataAccess.Models;

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
