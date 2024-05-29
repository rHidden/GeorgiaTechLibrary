using DataAccess.Models;
using GeorgiaTechLibrary.Services;
using GeorgiaTechLibrary.Services.ServiceInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> GetMember(string SSN)
        {
            var member = await _memberService.GetMember(SSN);
            if(member == null)
            {
                return NotFound();
            }
            return Ok(member);
        }

        [HttpGet]
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
        public async Task<IActionResult> CreateMember(Member member)
        {
            var createdMember = await _memberService.CreateMember(member);
            return Ok(createdMember);
        }

        [HttpPatch]
        [Route("{SSN}")]
        public async Task<IActionResult> UpdateMember(Member member)
        {
            var updatedMember = await _memberService.UpdateMember(member);
            return Ok(updatedMember);
        }

        [HttpDelete]
        [Route("{SSN}")]
        public async Task<IActionResult> DeleteMember(string SSN)
        {
            var deletedSuccessfully = await _memberService.DeleteMember(SSN);
            return Ok(deletedSuccessfully);
        }
    }
}
