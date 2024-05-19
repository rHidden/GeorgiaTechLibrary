using DataAccess.Repositories.RepositoryInterfaces;
using DataAccess.DAO;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using DataAccess.DAO.DAOIntefaces;

namespace DataAccess.Repositories
{
    public class MemberRepository : IMemberRepository
    {
        private readonly IDatabaseConnectionFactory _connectionFactory;
        public MemberRepository(IDatabaseConnectionFactory databaseConnectionFactory)
        {
            _connectionFactory = databaseConnectionFactory;
        }

        public async Task<Member> GetMember(string SSN)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Member>> ListMembers()
        {
            throw new NotImplementedException();
        }

        public async Task<Member> CreateMember(Member member)
        {
            throw new NotImplementedException();
        }
        public async Task<Member> DeleteMember(string SSN)
        {
            throw new NotImplementedException();
        }

        public async Task<Member> UpdateMember(Member newMember)
        {
            throw new NotImplementedException();
        }
    }
}
