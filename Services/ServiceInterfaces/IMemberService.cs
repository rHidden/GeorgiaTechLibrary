using GeorgiaTechLibrary.Models;

namespace GeorgiaTechLibrary.Services.ServiceInterfaces
{
    public interface IMemberService
    {
        Task<Member> CreateMember(Member member);
        Task DeleteMember(string SSN);
        Task UpdateMember(Member member);
        Task<Member> GetMember(string SSN);
        Task<List<Member>> ListMembers();

    }
}
