using DataAccess.DAO;
using DataAccess.Models;
using DataAccess.Repositories.RepositoryInterfaces;

namespace DataAccess.Repositories
{
    public class StaffRepository : IStaffRepository
    {
        private readonly GTLDbContext _context;

        public StaffRepository(GTLDbContext context)
        {
            _context = context;
        }

        public async Task<Staff> GetStaff(string SSN)
        {
            throw new NotImplementedException();
        }
        public async Task<List<Staff>> ListStaff()
        {
            throw new NotImplementedException();
        }

        public async Task<Staff> CreateStaff(Staff staff)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateStaff(Staff staff)
        {
            throw new NotImplementedException();
        }   

        public async Task DeleteStaff(string SSN)
        {
            throw new NotImplementedException();
        }
    }
}
