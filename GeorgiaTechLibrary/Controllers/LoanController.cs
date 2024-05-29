using DataAccess.Models;
using GeorgiaTechLibrary.DTOs;
using GeorgiaTechLibrary.Services.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

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
        [SwaggerOperation(Summary = "Get loan",
            Description = "Returns a loan based on the passed ID.\n\n" +
            "param id - Identifier of the loan")]
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
        [SwaggerOperation(Summary = "List user loans",
            Description = "Returns a list of loans for a specific user based on the user's SSN.\n\n" +
            "param userSSN - Social Security Number of the user")]
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
        [SwaggerOperation(Summary = "Create a new book loan",
            Description = "Creates a new loan for a book and returns the created loan.\n\n" +
            "param loan - The book loan to be created")]
        public async Task<IActionResult> CreateLoan(BookLoan loan)
        {
            var createdLoan = await _loanService.CreateLoan(loan);
            return Ok(createdLoan);
        }

        [HttpPost]
        [Route("DigitalItem")]
        [SwaggerOperation(Summary = "Create a new digital item loan",
            Description = "Creates a new loan for a digital item and returns the created loan.\n\n" +
            "param loan - The digital item loan to be created")]
        public async Task<IActionResult> CreateLoan(DigitalItemLoan loan)
        {
            var createdLoan = await _loanService.CreateLoan(loan);
            return Ok(createdLoan);
        }

        [HttpPatch]
        [Route("{id}")]
        [SwaggerOperation(Summary = "Update a loan",
            Description = "Updates the details of a loan.\n\n" +
            "param loan - The updated loan details")]
        public async Task<IActionResult> UpdateLoan(Loan loan)
        {
            var updatedLoan = await _loanService.UpdateLoan(loan);
            return Ok(updatedLoan);
        }

        [HttpDelete]
        [Route("{id}")]
        [SwaggerOperation(Summary = "Delete a loan",
            Description = "Deletes a loan based on the passed ID.\n\n" +
            "param id - Identifier of the loan")]
        public async Task<IActionResult> DeleteLoan(int id)
        {
            var deletedSuccessfully = await _loanService.DeleteLoan(id);
            return Ok(deletedSuccessfully);
        }
    }
}
