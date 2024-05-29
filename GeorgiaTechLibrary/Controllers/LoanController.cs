﻿using DataAccess.Models;
using GeorgiaTechLibrary.DTOs;
using GeorgiaTechLibrary.Services;
using GeorgiaTechLibrary.Services.ServiceInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GeorgiaTechLibrary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoanController : ControllerBase
    {
        private readonly ILoanService _loanService;
        public LoanController(ILoanService loanService)
        {
            _loanService = loanService;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetLoan(int id)
        {
            var loan = await _loanService.GetLoan(id);
            if (loan == null)
            {
                return NotFound();
            }
            return Ok(loan);
        }

        [HttpGet]
        [Route("user/{userSSN}")]
        public async Task<IActionResult> ListUserLoans(string userSSN)
        {
            List<LoanDTO> loans = await _loanService.ListUserLoans(userSSN);
            if (!loans.Any())
            {
                return NotFound();
            }
            return Ok(loans);
        }

        [HttpPost]
        [Route("Book")]
        public async Task<IActionResult> CreateLoan(BookLoan loan)
        {
            var createdLoan = await _loanService.CreateLoan(loan);
            return Ok(createdLoan);
        }

        [HttpPost]
        [Route("DigitalItem")]
        public async Task<IActionResult> CreateLoan(DigitalItemLoan loan)
        {
            var createdLoan = await _loanService.CreateLoan(loan);
            return Ok(createdLoan);
        }

        [HttpPatch]
        [Route("{id}")]
        public async Task<IActionResult> UpdateLoan(Loan loan)
        {
            var updatedLoan = await _loanService.UpdateLoan(loan);
            return Ok(updatedLoan);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteLoan(int Id)
        {
            var deletedSuccessfully = await _loanService.DeleteLoan(Id);
            return Ok(deletedSuccessfully);
        }
    }
}
