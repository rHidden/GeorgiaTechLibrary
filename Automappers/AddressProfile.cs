using AutoMapper;
using GeorgiaTechLibrary.DTOs;
using GeorgiaTechLibrary.Models;

namespace GeorgiaTechLibrary.Automappers
{
    public class AddressProfile : Profile
    {
        public AddressProfile()
        {
            CreateMap<Address, AddressDTO>().ReverseMap();
        }
    }
}
