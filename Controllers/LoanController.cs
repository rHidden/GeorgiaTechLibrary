using GeorgiaTechLibrary.Models;
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
        public async Task<IActionResult> GetLoan(int Id)
        {
            var loan = await _loanService.GetLoan(Id);
            return Ok(loan);
        }

        [HttpGet]
        [Route("{userSSN}")]
        public async Task<IActionResult> ListUserLoans(string userSSN)
        {
            List<Loan> loans = await _loanService.ListUserLoans(userSSN);
            if (!loans.Any())
            {
                return NotFound();
            }
            return Ok(loans);
        }

        [HttpPost]
        public async Task<IActionResult> CreateLoan(Loan loan)
        {
            var createdLoan = await _loanService.CreateLoan(loan);
            return Ok(createdLoan);
        }

        [HttpPatch]
        public IActionResult UpdateLoan(Loan loan)
        {
            _loanService.UpdateLoan(loan);
            return Ok();
        }

        [HttpDelete]
        public IActionResult DeleteLoan(int Id)
        {
            _loanService.DeleteLoan(Id);
            return Ok();
        }
    }
}
