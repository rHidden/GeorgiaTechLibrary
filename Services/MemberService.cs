using GeorgiaTechLibrary.Models;
using GeorgiaTechLibrary.Repositories;
using GeorgiaTechLibrary.Repositories.RepositoryInterfaces;
using GeorgiaTechLibrary.Services.ServiceInterfaces;

namespace GeorgiaTechLibrary.Services
{
    public class MemberService : IMemberService
    {
        private readonly IMemberRepository _memberRepository;

        public MemberService(IMemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
        }
        public async Task<List<Member>> ListMembers()
        {
            return await _memberRepository.ListMembers();
        }

        public async Task<Member> CreateMember(Member member)
        {
            return await _memberRepository.CreateMember(member);
        }

        public async Task DeleteMember(int SSN)
        {
            await _memberRepository.DeleteMember(SSN);

        }

        public async Task<Member> GetMember(int SSN)
        {
            return await _memberRepository.GetMember(SSN);
        }

        public async Task UpdateMember(Member member)
        {
            await _memberRepository.UpdateMember(member);
        }
    }
}
