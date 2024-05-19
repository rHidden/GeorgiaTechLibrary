using DataAccess.Models;

namespace DataAccess.Repositories.RepositoryInterfaces
{
    public interface ILoanRepository
    {
        Task<Loan> GetLoan(int id);
        Task<List<Loan>> ListUserLoans(string userSSN);
        Task<Loan> CreateLoan(Loan loan);
        Task<Loan> UpdateLoan(Loan loan); 
        Task<Loan> DeleteLoan(int id);
    }
}
