using DataAccess.Models;
using GeorgiaTechLibrary.DTOs;

namespace GeorgiaTechLibrary.Services.ServiceInterfaces
{
    public interface IStaffService
    {
        Task<Staff> GetStaff(string SSN);
        Task<List<Staff>> ListStaff();
        Task<Staff> CreateStaff(Staff staff);
        Task<Staff> UpdateStaff(Staff staff);
        Task<bool> DeleteStaff(string SSN);
        Task<List<StaffOutsideCityDto>> GetStaffLivingOutsideOfCityPerLibrary();
        Task<List<StaffOutsideCityDto>> GetStaffLivingOutsideOfCity();
    }
}
