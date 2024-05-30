using AutoMapper;
using GeorgiaTechLibrary.DTOs;
using DataAccess.Models;
using System.Collections.Generic;

namespace GeorgiaTechLibrary.Automappers
{
    public class LoanItemStatisticsProfile : Profile
    {
        public LoanItemStatisticsProfile()
        {
            CreateMap<(double Books, double Videos, double Audios, double Texts, double Images), LoanItemStatisticsDTO>()
                .ForMember(dest => dest.Books, opt => opt.MapFrom(src => src.Books))
                .ForMember(dest => dest.Videos, opt => opt.MapFrom(src => src.Videos))
                .ForMember(dest => dest.Audios, opt => opt.MapFrom(src => src.Audios))
                .ForMember(dest => dest.Texts, opt => opt.MapFrom(src => src.Texts))
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images));
        }
    }
}
