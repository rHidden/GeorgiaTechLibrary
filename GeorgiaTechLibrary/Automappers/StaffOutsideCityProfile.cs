using AutoMapper;
using GeorgiaTechLibrary.DTOs;
using DataAccess.Models;
using System.Collections.Generic;

namespace GeorgiaTechLibrary.Automappers
{
    public class StaffOutsideCityProfile : Profile
    {
        public StaffOutsideCityProfile()
        {
            CreateMap<(string Name, int StaffLivingOutsideOfCity), StaffOutsideCityDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.StaffLivingOutsideOfCity, opt => opt.MapFrom(src => src.StaffLivingOutsideOfCity));
        }
    }
}
