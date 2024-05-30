using DataAccess.Models;
using GeorgiaTechLibrary.Services.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace GeorgiaTechLibrary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly IMemberService _memberService;
        public MemberController(IMemberService memberService)
        {
            _memberService = memberService;
        }

        [HttpGet]
        [Route("{SSN}")]
        [SwaggerOperation(Summary = "Get member",
            Description = "Returns a member based on the passed SSN.\n\n" +
            "param SSN - Social Security Number of the member")]
        public async Task<IActionResult> GetMember(string SSN)
        {
            var member = await _memberService.GetMember(SSN);
            if (member == null)
            {
                return NotFound();
            }
            return Ok(member);
        }

        [HttpGet]
        [SwaggerOperation(Summary = "List all members",
            Description = "Returns a list of all members.")]
        public async Task<IActionResult> ListMembers()
        {
            List<Member> members = await _memberService.ListMembers();
            if (!members.Any())
            {
                return NotFound();
            }
            return Ok(members);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Create a new member",
            Description = "Creates a new member and returns the created member.\n\n" +
            "param member - The created member")]
        public async Task<IActionResult> CreateMember(Member member)
        {
            var createdMember = await _memberService.CreateMember(member);
            return Ok(createdMember);
        }

        [HttpPatch]
        [Route("{SSN}")]
        [SwaggerOperation(Summary = "Update a member",
            Description = "Updates the details of a member.\n\n" +
            "param member - The updated member")]
        public async Task<IActionResult> UpdateMember(Member member)
        {
            var updatedMember = await _memberService.UpdateMember(member);
            return Ok(updatedMember);
        }

        [HttpDelete]
        [Route("{SSN}")]
        [SwaggerOperation(Summary = "Delete a member",
            Description = "Deletes a member based on the passed SSN.\n\n" +
            "param SSN - Social Security Number of the member")]
        public async Task<IActionResult> DeleteMember(string SSN)
        {
            var deletedSuccessfully = await _memberService.DeleteMember(SSN);
            return Ok(deletedSuccessfully);
        }
    }
}
