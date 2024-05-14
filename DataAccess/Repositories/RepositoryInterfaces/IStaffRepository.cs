using DataAccess.Models;

namespace DataAccess.Repositories.RepositoryInterfaces
{
    public interface IStaffRepository
    {
        Task<Staff> GetStaff(string SSN);
        Task<List<Staff>> ListStaff();
        Task<Staff> CreateStaff(Staff staff);
        Task UpdateStaff(Staff staff);
        Task DeleteStaff(string SSN);
    }
}
