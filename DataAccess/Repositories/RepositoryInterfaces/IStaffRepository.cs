using DataAccess.Models;

namespace DataAccess.Repositories.RepositoryInterfaces
{
    public interface IStaffRepository
    {
        Task<Staff> GetStaff(string SSN);
        Task<List<Staff>> ListStaff();
        Task<Staff> CreateStaff(Staff staff);
        Task<Staff> UpdateStaff(Staff staff);
        Task<Staff> DeleteStaff(string SSN);
    }
}
