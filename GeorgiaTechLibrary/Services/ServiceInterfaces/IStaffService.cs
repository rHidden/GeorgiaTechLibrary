using DataAccess.Models;

namespace GeorgiaTechLibrary.Services.ServiceInterfaces
{
    public interface IStaffService
    {
        Task<Staff> GetStaff(string SSN);
        Task<List<Staff>> ListStaff();
        Task<Staff> CreateStaff(Staff staff);
        Task<Staff> UpdateStaff(Staff staff);
        Task<Staff> DeleteStaff(string SSN);

    }
}
