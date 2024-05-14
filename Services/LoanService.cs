using GeorgiaTechLibrary.Models;
using GeorgiaTechLibrary.Repositories;
using GeorgiaTechLibrary.Repositories.RepositoryInterfaces;
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

        public async Task<Loan> CreateLoan(Loan loan)
        {
            return await _loanRepository.CreateLoan(loan);
        }

        public async Task UpdateLoan(Loan loan)
        {
            await _loanRepository.UpdateLoan(loan);
        }

        public async Task DeleteLoan(int Id)
        {
            await _loanRepository.DeleteLoan(Id);

        }
    }
}
