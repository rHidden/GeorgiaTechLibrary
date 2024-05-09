using GeorgiaTechLibrary.Models;

namespace GeorgiaTechLibrary.Repositories.RepositoryInterfaces
{
    public interface IMemberRepository
    {
        Task<Member> CreateMember(Member member);
        Task DeleteMember(int SSN);
        Task UpdateMember(Member member);
        Task<Member> GetMember(int SSN);
        Task<List<Member>> ListMembers();
    }
}
