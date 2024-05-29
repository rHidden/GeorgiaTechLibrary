using DataAccess.Models;

namespace DataAccess.Repositories.RepositoryInterfaces
{
    public interface ILoanRepository
    {
        Task<Loan?> GetLoan(int id);
        Task<List<Loan>> ListUserLoans(string userSSN);
        Task<Loan?> CreateLoan(DigitalItemLoan loan);
        Task<Loan?> CreateLoan(BookLoan loan);
        Task<Loan> UpdateLoan(Loan loan); 
        Task<bool> DeleteLoan(int id);
        Task<Loan?> ReturnLoan(int id);
        Task<(double Books, double Videos, double Audios, double Texts, double Images)> GetLoanItemStatistics();
    }
}
