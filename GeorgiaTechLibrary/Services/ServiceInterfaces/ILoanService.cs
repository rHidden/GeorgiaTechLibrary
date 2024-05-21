using DataAccess.Models;

namespace GeorgiaTechLibrary.Services.ServiceInterfaces
{
    public interface ILoanService
    {
        Task<Loan> GetLoan(int Id);
        Task<List<Loan>> ListUserLoans(string userSSN);
        Task<Loan> CreateLoan(DigitalItemLoan loan);
        Task<Loan> CreateLoan(BookLoan loan);
        Task<Loan> UpdateLoan(Loan loan);
        Task<bool> DeleteLoan(int Id);

    }
}
