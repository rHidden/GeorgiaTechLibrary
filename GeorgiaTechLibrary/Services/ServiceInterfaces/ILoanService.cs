using DataAccess.Models;
using GeorgiaTechLibrary.DTOs;

namespace GeorgiaTechLibrary.Services.ServiceInterfaces
{
    public interface ILoanService
    {
        Task<LoanDTO> GetLoan(int id);
        Task<List<LoanDTO>> ListUserLoans(string userSSN);
        Task<Loan?> CreateLoan(DigitalItemLoan loan);
        Task<Loan?> CreateLoan(BookLoan loan);
        Task<Loan> UpdateLoan(Loan loan);
        Task<bool> DeleteLoan(int id);
        Task<Loan> ReturnLoan(int id);
    }
}
