using GeorgiaTechLibrary.Models;

namespace GeorgiaTechLibrary.Services.ServiceInterfaces
{
    public interface ILoanService
    {
        Task<Loan> GetLoan(int Id);
        Task<List<Loan>> ListUserLoans(string userSSN);
        Task<Loan> CreateLoan(Loan loan);
        Task UpdateLoan(Loan loan);
        Task DeleteLoan(int Id);

    }
}
