using GeorgiaTechLibrary.Models;

namespace GeorgiaTechLibrary.Services.ServiceInterfaces
{
    public interface IMemberService
    {
        Task<Member> GetMember(string SSN);
        Task<List<Member>> ListMembers();
        Task<Member> CreateMember(Member member);
        Task UpdateMember(Member member);
        Task DeleteMember(string SSN);

    }
}
