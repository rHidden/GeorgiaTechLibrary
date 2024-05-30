using AutoMapper;
using DataAccess.Models;
using DataAccess.Repositories;
using DataAccess.Repositories.RepositoryInterfaces;
using GeorgiaTechLibrary.DTOs;
using GeorgiaTechLibrary.Services.ServiceInterfaces;

namespace GeorgiaTechLibrary.Services
{
    public class LoanService : ILoanService
    {
        private readonly ILoanRepository _loanRepository;
        private readonly IMapper _mapper;

        public LoanService(ILoanRepository loanRepository, IMapper mapper)
        {
            _loanRepository = loanRepository;
            _mapper = mapper;
        }

        public async Task<LoanDTO> GetLoan(int Id)
        {
            var loan = await _loanRepository.GetLoan(Id);
            return _mapper.Map<LoanDTO>(loan);
        }

        public async Task<List<LoanDTO>> ListUserLoans(string userSSN)
        {
            return _mapper.Map<List<LoanDTO>>(await _loanRepository.ListUserLoans(userSSN));
        }

        public async Task<Loan?> CreateLoan(BookLoan loan)
        {
            return await _loanRepository.CreateLoan(loan);
        }

        public async Task<Loan?> CreateLoan(DigitalItemLoan loan)
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

        public async Task<float> GetAverageNumberOfDaysToReturnBooks()
        {
            return await _loanRepository.GetAverageNumberOfDaysToReturnBooks();
        }

    }
}
