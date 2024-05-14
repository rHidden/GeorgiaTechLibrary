using GeorgiaTechLibrary.Models;

namespace GeorgiaTechLibrary.Repositories.RepositoryInterfaces
{
    public interface ILoanRepository
    {
        Task<Loan> GetLoan(int id);
        Task<List<Loan>> ListUserLoans(string userSSN);
        Task<Loan> CreateLoan(Loan loan);
        Task UpdateLoan(Loan loan); 
        Task DeleteLoan(int id);
    }
}
