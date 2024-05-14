using DataAccess.Models;

namespace GeorgiaTechLibrary.Services.ServiceInterfaces
{
    public interface IStaffService
    {
        Task<Staff> GetStaff(string SSN);
        Task<List<Staff>> ListStaff();
        Task<Staff> CreateStaff(Staff staff);
        Task UpdateStaff(Staff staff);
        Task DeleteStaff(string SSN);

    }
}
