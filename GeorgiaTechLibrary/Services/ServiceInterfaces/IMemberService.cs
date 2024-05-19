using DataAccess.Models;

namespace GeorgiaTechLibrary.Services.ServiceInterfaces
{
    public interface IMemberService
    {
        Task<Member> GetMember(string SSN);
        Task<List<Member>> ListMembers();
        Task<Member> CreateMember(Member member);
        Task<Member> UpdateMember(Member member);
        Task<Member> DeleteMember(string SSN);

    }
}
