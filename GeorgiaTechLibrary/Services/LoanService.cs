using DataAccess.Models;
using DataAccess.Repositories;
using DataAccess.Repositories.RepositoryInterfaces;
using GeorgiaTechLibrary.Services.ServiceInterfaces;

namespace GeorgiaTechLibrary.Services
{
    public class LoanService : ILoanService
    {
        private readonly ILoanRepository _loanRepository;

        public LoanService(ILoanRepository loanRepository)
        {
            _loanRepository = loanRepository;
        }

        public async Task<Loan> GetLoan(int Id)
        {
            return await _loanRepository.GetLoan(Id);
        }

        public async Task<List<Loan>> ListUserLoans(string userSSN)
        {
            return await _loanRepository.ListUserLoans(userSSN);
        }

        public async Task<Loan> CreateLoan(BookLoan loan)
        {
            return await _loanRepository.CreateLoan(loan);
        }

        public async Task<Loan> CreateLoan(DigitalItemLoan loan)
        {
            return await _loanRepository.CreateLoan(loan);
        }

        public async Task<Loan> UpdateLoan(Loan loan)
        {
            return await _loanRepository.UpdateLoan(loan);
        }

        public async Task<bool> DeleteLoan(int Id)
        {
            return await _loanRepository.DeleteLoan(Id);
        }
    }
}
