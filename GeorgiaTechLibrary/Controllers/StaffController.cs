using DataAccess.Models;
using GeorgiaTechLibrary.Services;
using GeorgiaTechLibrary.Services.ServiceInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GeorgiaTechLibrary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaffController : ControllerBase
    {
        private readonly IStaffService _staffService;
        public StaffController(IStaffService staffService)
        {
            _staffService = staffService;
        }   

        [HttpGet]
        [Route("{SSN}")]
        public async Task<IActionResult> GetStaff(string SSN)
        {
            var staff = await _staffService.GetStaff(SSN);
            return Ok(staff);
        }

        [HttpGet]
        public async Task<IActionResult> ListStaff()
        {
            List<Staff> staffs = await _staffService.ListStaff();
            if (!staffs.Any())
            {
                return NotFound();
            }
            return Ok(staffs);
        }

        [HttpPost]
        public async Task<IActionResult> CreateStaff(Staff staff)
        {
            var createdStaff = await _staffService.CreateStaff(staff);
            return Ok(createdStaff);
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateStaff(Staff staff)
        {
            var updatedStaff = await _staffService.UpdateStaff(staff);
            return Ok(updatedStaff);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteStaff(string SSN)
        {
            var deletedSuccessfully = await _staffService.DeleteStaff(SSN);
            return Ok(deletedSuccessfully);
        }
    }
}
