using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Order.Application.Features.Dtos;
using Webshop.Order.Application.Features.Requests;

namespace Webshop.Order.Application.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Domain.AggregateRoots.OrderLine, OrderLineDto>().ReverseMap();
            CreateMap<Domain.AggregateRoots.OrderLine, CreateOrderLineRequest>().ReverseMap();
            CreateMap<Domain.AggregateRoots.OrderLine, UpdateOrderLineRequest>().ReverseMap();
            CreateMap<Domain.AggregateRoots.Order, OrderDto>().ForMember(dest => dest.OrderLines, 
                opt => opt.MapFrom(src => src.OrderLines)).ReverseMap();
            CreateMap<Domain.AggregateRoots.Order, CreateOrderRequest>().ForMember(dest => dest.OrderLines,
                opt => opt.MapFrom(src => src.OrderLines)).ReverseMap(); ;
            CreateMap<Domain.AggregateRoots.Order, UpdateOrderRequest>().ForMember(dest => dest.OrderLines,
                opt => opt.MapFrom(src => src.OrderLines)).ReverseMap(); ;
        }
    }
}
