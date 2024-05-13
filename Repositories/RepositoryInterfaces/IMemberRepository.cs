using GeorgiaTechLibrary.Models;

namespace GeorgiaTechLibrary.Repositories.RepositoryInterfaces
{
    public interface IMemberRepository
    {
        Task<Member> CreateMember(Member member);
        Task<Member> GetMember(string SSN);
        Task UpdateMember(Member member);
        Task DeleteMember(string SSN);
        Task<List<Member>> ListMembers();
    }
}
