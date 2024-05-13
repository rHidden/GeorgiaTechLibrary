using GeorgiaTechLibrary.Models;

namespace GeorgiaTechLibrary.Repositories.RepositoryInterfaces
{
    public interface IStaffRepository
    {
        Task<Staff> CreateStaff(Staff staff);
        Task<Staff> GetStaff(int SSN);
        Task UpdateStaff(Staff staff);
        Task DeleteStaff(int SSN);
        Task<List<Staff>> ListStaff();
    }
}
