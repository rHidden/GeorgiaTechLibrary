using DataAccess.Repositories.RepositoryInterfaces;
using DataAccess.Models;
using Dapper;
using DataAccess.DAO.DAOIntefaces;
using System.Runtime.Intrinsics.X86;

namespace DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDatabaseConnectionFactory _connectionFactory;

        public UserRepository(IDatabaseConnectionFactory databaseConnectionFactory)
        {
            _connectionFactory = databaseConnectionFactory;
        }

        public async Task<List<User>> GetMostActiveUsers()
        {
            string sql = "EXECUTE dbo.GetMostActiveUsers;";
            using (var connection = _connectionFactory.CreateConnection())
            {
                var users = (await connection.QueryAsync<User, Address, User>(sql, map: (user, address) =>
                {
                    user.UserAddress = address;
                    return user;
                },
                splitOn: "Street")).ToList();

                return users;
            }
        }
    }
}
