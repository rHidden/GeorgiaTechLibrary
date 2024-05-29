using DataAccess.Models;
using GeorgiaTechLibrary.Services.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

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
        [SwaggerOperation(Summary = "Get staff member",
            Description = "Returns a staff member based on the passed SSN.\n\n" +
            "param SSN - Social Security Number of the staff member")]
        public async Task<IActionResult> GetStaff(string SSN)
        {
            var staff = await _staffService.GetStaff(SSN);
            if (staff == null)
            {
                return NotFound();
            }
            return Ok(staff);
        }

        [HttpGet]
        [SwaggerOperation(Summary = "List all staff members",
            Description = "Returns a list of all staff members.")]
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
        [SwaggerOperation(Summary = "Create a new staff member",
            Description = "Creates a new staff member and returns the created staff member.\n\n" +
            "param staff - The created staff member")]
        public async Task<IActionResult> CreateStaff(Staff staff)
        {
            var createdStaff = await _staffService.CreateStaff(staff);
            return Ok(createdStaff);
        }

        [HttpPatch]
        [Route("{SSN}")]
        [SwaggerOperation(Summary = "Update a staff member",
            Description = "Updates the details of a staff.\n\n" +
            "param staff - The updated staff member")]
        public async Task<IActionResult> UpdateStaff(Staff staff)
        {
            var updatedStaff = await _staffService.UpdateStaff(staff);
            return Ok(updatedStaff);
        }

        [HttpDelete]
        [Route("{SSN}")]
        [SwaggerOperation(Summary = "Delete a staff member",
            Description = "Deletes a staff member based on the passed SSN.\n\n" +
            "param SSN - Social Security Number of the staff member")]
        public async Task<IActionResult> DeleteStaff(string SSN)
        {
            var deletedSuccessfully = await _staffService.DeleteStaff(SSN);
            return Ok(deletedSuccessfully);
        }
    }
}
