using DataAccess.DAO;
using DataAccess.DAO.DAOIntefaces;
using DataAccess.Models;
using DataAccess.Repositories.RepositoryInterfaces;

namespace DataAccess.Repositories
{
    public class StaffRepository : IStaffRepository
    {
        private readonly IDatabaseConnectionFactory _connectionFactory;
        public StaffRepository(IDatabaseConnectionFactory databaseConnectionFactory)
        {
            _connectionFactory = databaseConnectionFactory;
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
