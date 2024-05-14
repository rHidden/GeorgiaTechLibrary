using DataAccess.DAO;
using DataAccess.Models;
using DataAccess.Repositories.RepositoryInterfaces;

namespace DataAccess.Repositories
{
    public class LoanRepository : ILoanRepository
    {
        private readonly GTLDbContext _context;

        public LoanRepository(GTLDbContext context)
        {
            _context = context;
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
