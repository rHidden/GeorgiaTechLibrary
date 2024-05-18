using DataAccess.DAO;
using DataAccess.DAO.DAOIntefaces;
using DataAccess.Models;
using DataAccess.Repositories.RepositoryInterfaces;

namespace DataAccess.Repositories
{
    public class LoanRepository : ILoanRepository
    {
        private readonly IDatabaseConnectionFactory _connectionFactory;
        public LoanRepository(IDatabaseConnectionFactory databaseConnectionFactory)
        {
            _connectionFactory = databaseConnectionFactory;
        }

        public Task<Loan> GetLoan(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Loan>> ListUserLoans(string userSSN)
        {
            throw new NotImplementedException();
        }

        public Task<Loan> CreateLoan(Loan loan)
        {
            throw new NotImplementedException();
        }

        public Task UpdateLoan(Loan loan)
        {
            throw new NotImplementedException();
        }

        public Task DeleteLoan(int id)
        {
            throw new NotImplementedException();
        }
    }
}
