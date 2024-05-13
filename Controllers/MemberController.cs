using GeorgiaTechLibrary.Models;
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
        public async Task<IActionResult> GetMember(string SSN)
        {
            var member = await _memberService.GetMember(SSN);
            return Ok(member);
        }

        [HttpGet]
        [Route("list")]
        public async Task<IActionResult> ListMembersAsync()
        {
            List<Member> members = await _memberService.ListMembers();
            if (!members.Any())
            {
                return NotFound();
            }
            return Ok(members);
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateMember(Member member)
        {
            var createdMember = await _memberService.CreateMember(member);
            return Ok(createdMember);
        }

        [HttpPatch]
        public IActionResult UpdateMember(Member member)
        {
            _memberService.UpdateMember(member);
            return Ok();
        }

        [HttpDelete]
        public IActionResult DeleteMember(string SSN)
        {
            _memberService.DeleteMember(SSN);
            return Ok();
        }
    }
}
