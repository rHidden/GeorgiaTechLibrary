using AutoMapper;
using DataAccess.Models;
using DataAccess.Repositories;
using DataAccess.Repositories.RepositoryInterfaces;
using GeorgiaTechLibrary.DTOs;
using GeorgiaTechLibrary.Services.ServiceInterfaces;

namespace GeorgiaTechLibrary.Services
{
    public class StaffService : IStaffService
    {
        private readonly IStaffRepository _staffRepository;
        private readonly IMapper _mapper;

        public StaffService(IStaffRepository staffRepository, IMapper mapper)
        {
            _staffRepository = staffRepository;
            _mapper = mapper;
        }

        public async Task<Staff> GetStaff(string SSN)
        {
            return await _staffRepository.GetStaff(SSN);
        }

        public async Task<List<Staff>> ListStaff()
        {
            return await _staffRepository.ListStaff();
        }

        public async Task<Staff> CreateStaff(Staff staff)
        {
            return await _staffRepository.CreateStaff(staff);
        }

        public async Task<Staff> UpdateStaff(Staff staff)
        {
            return await _staffRepository.UpdateStaff(staff);
        }

        public async Task<bool> DeleteStaff(string SSN)
        {
            return await _staffRepository.DeleteStaff(SSN);
        }

        public async Task<List<StaffOutsideCityDto>> GetStaffLivingOutsideOfCityPerLibrary()
        {
            return (await _staffRepository.GetStaffLivingOutsideOfCityPerLibrary()).Select(result => _mapper.Map<StaffOutsideCityDto>(result)).ToList();
        }

        public async Task<List<StaffOutsideCityDto>> GetStaffLivingOutsideOfCity()
        {
            return (await _staffRepository.GetStaffLivingOutsideOfCity()).Select(result => _mapper.Map<StaffOutsideCityDto>(result)).ToList();
        }
    }
}
