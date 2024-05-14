using DataAccess.Models;
using DataAccess.Repositories;
using DataAccess.Repositories.RepositoryInterfaces;
using GeorgiaTechLibrary.Services.ServiceInterfaces;

namespace GeorgiaTechLibrary.Services
{
    public class StaffService : IStaffService
    {
        private readonly IStaffRepository _staffRepository;

        public StaffService(IStaffRepository staffRepository)
        {
            _staffRepository = staffRepository;
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

        public async Task UpdateStaff(Staff staff)
        {
            await _staffRepository.UpdateStaff(staff);
        }

        public async Task DeleteStaff(string SSN)
        {
            await _staffRepository.DeleteStaff(SSN);

        }
    }
}
