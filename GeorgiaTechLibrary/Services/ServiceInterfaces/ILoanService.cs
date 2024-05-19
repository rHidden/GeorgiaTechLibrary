﻿using DataAccess.Models;

namespace GeorgiaTechLibrary.Services.ServiceInterfaces
{
    public interface ILoanService
    {
        Task<Loan> GetLoan(int Id);
        Task<List<Loan>> ListUserLoans(string userSSN);
        Task<Loan> CreateLoan(Loan loan);
        Task<Loan> UpdateLoan(Loan loan);
        Task<Loan> DeleteLoan(int Id);

    }
}
