using GeorgiaTechLibrary.Models;

namespace GeorgiaTechLibrary.Repositories.RepositoryInterfaces
{
    public interface ILoanRepository
    {
        Task<Loan> CreateLoan(Loan loan);
        Task<Loan> GetLoan(int id);
        Task UpdateLoan(Loan loan);
        Task DeleteLoan(int id);  //Should we actually allow?
        Task<List<Loan>> ListUserLoans(string userSSN);
    }
}
