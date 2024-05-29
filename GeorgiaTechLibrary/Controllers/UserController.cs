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
        [Route("active")]
        [SwaggerOperation(Summary = "Get most active users",
            Description = "Returns a list of the most active users.")]
        public async Task<IActionResult> GetMostActiveUsers()
        {
            var users = await _userService.GetMostActiveUsers();
            return Ok(users);
        }
    }
}