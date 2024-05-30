using DataAccess.Models;
using GeorgiaTechLibrary.Services.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace GeorgiaTechLibrary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Route("Active")]
        [SwaggerOperation(Summary = "Get most active users",
            Description = "Returns a list of the most active users.")]
        public async Task<IActionResult> GetMostActiveUsers()
        {
            var users = await _userService.GetMostActiveUsers();
            return Ok(users);
        }

        [HttpGet]
        [Route("Late")]
        [SwaggerOperation(Summary = "Get days late",
            Description = "Returns a list of users with the number of days they are late in returning items in total.")]
        public async Task<IActionResult> GetDaysLate()
        {
            var users = await _userService.GetDaysLate();
            return Ok(users);
        }

        [HttpGet]
        [Route("AverageDuration")]
        [SwaggerOperation(Summary = "Get average loan duration",
            Description = "Returns the average duration of loans for all users.")]
        public async Task<IActionResult> AverageDuration()
        {
            var users = await _userService.GetAverageLoanDuration();
            return Ok(users);
        }
    }
}