using GeorgiaTechLibrary.Models;

namespace GeorgiaTechLibrary.Services.ServiceInterfaces
{
    public interface IMemberService
    {
        Task<Member> CreateMember(Member member);
        Task DeleteMember(int SSN);
        Task UpdateMember(Member member);
        Task<Member> GetMember(int SSN);
        Task<List<Member>> ListMembers();

    }
}
