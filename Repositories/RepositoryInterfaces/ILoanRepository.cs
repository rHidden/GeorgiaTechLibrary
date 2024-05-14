using GeorgiaTechLibrary.Models;

namespace GeorgiaTechLibrary.Repositories.RepositoryInterfaces
{
    public interface ILoanRepository
    {
        Task<Loan> CreateLoan(Loan loan);
        Task<Loan> GetLoan(int id);
        Task UpdateLoan(Loan loan);
        Task DeleteLoan(int id);
        Task<List<Loan>> ListUserLoans(string userSSN);
    }
}
