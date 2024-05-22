using DataAccess.Repositories.RepositoryInterfaces;
using DataAccess.Models;
using Dapper;
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
            string sql = @"
                SELECT u.SSN, u.PhoneNumber, u.FirstName, u.LastName, u.LibraryName, u.Street, u.StreetNumber, u.City, u.Zipcode, m.CardNumber, m.ExpiryDate, m.Photo, m.MemberType
                FROM [User] u
                INNER JOIN [Member] m ON u.SSN = m.UserSSN
                WHERE u.SSN = @SSN";

            using (var connection = _connectionFactory.CreateConnection())
            {
                var member = (await connection.QueryAsync<User, Address, Member, Member>(sql, (user, address, member) =>
                {
                    member.SSN = user.SSN;
                    member.FirstName = user.FirstName;
                    member.LastName = user.LastName;
                    member.PhoneNumber = user.PhoneNumber;
                    member.UserAddress = address;
                    return member;
                }, new { SSN }, splitOn: "Street, CardNumber")).AsQueryable().FirstOrDefault();

                return member;
            }
        }

        public async Task<List<Member>> ListMembers()
        {
            string sql = @"
                SELECT u.SSN, u.PhoneNumber, u.FirstName, u.LastName, u.LibraryName, u.Street, u.StreetNumber, u.City, u.Zipcode, m.CardNumber, m.ExpiryDate, m.Photo, m.MemberType
                FROM [User] u
                INNER JOIN [Member] m ON u.SSN = m.UserSSN";

            using (var connection = _connectionFactory.CreateConnection())
            {
                var members = await connection.QueryAsync<User, Address, Member, Member>(sql, (user, address, member) =>
                {
                    member.SSN = user.SSN;
                    member.FirstName = user.FirstName;
                    member.LastName = user.LastName;
                    member.PhoneNumber = user.PhoneNumber;
                    member.UserAddress = address;
                    return member;
                }, splitOn: "Street, CardNumber");

                return members.ToList();
            }
        }

        public async Task<Member> CreateMember(Member member)
        {
            string sqlUser = "INSERT INTO [User] (SSN, FirstName, LastName, PhoneNumber, Street, StreetNumber, City, Zipcode) " +
                "VALUES (@SSN, @FirstName, @LastName, @PhoneNumber, @Street, @StreetNumber, @City, @Zipcode)";

            string sqlMember = "INSERT INTO [Member] (UserSSN, CardNumber, ExpiryDate, Photo, MemberType) " +
                "VALUES (@SSN, @CardNumber, @ExpiryDate, @Photo, @MemberType)";

            using (var connection = _connectionFactory.CreateConnection())
            {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction())
                {
                    await connection.ExecuteAsync(sqlUser, new { 
                        SSN = member.SSN,
                        FirstName = member.FirstName,
                        LastName = member.LastName,
                        PhoneNumber = member.PhoneNumber,
                        Street = member.UserAddress.Street,
                        StreetNumber = member.UserAddress.StreetNumber,
                        City = member.UserAddress.City,
                        ZipCode = member.UserAddress.ZipCode 
                    }, transaction);
                    await connection.ExecuteAsync(sqlMember, member, transaction);
                    transaction.Commit();
                }
            }
            return member;
        }

        public async Task<Member> UpdateMember(Member member)
        {
            string sqlUser = "UPDATE [User] SET " +
                "FirstName = @FirstName, LastName = @LastName, PhoneNumber = @PhoneNumber, Street = @Street, StreetNumber = @StreetNumber, City = @City, Zipcode = @Zipcode " +
                "WHERE SSN = @SSN";

            string sqlMember = "UPDATE [Member] SET " +
                "CardNumber = @CardNumber, ExpiryDate = @ExpiryDate, Photo = @Photo, MemberType = @MemberType " +
                "WHERE UserSSN = @SSN";

            using (var connection = _connectionFactory.CreateConnection())
            {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction())
                {
                    await connection.ExecuteAsync(sqlUser, new
                    {
                        SSN = member.SSN,
                        FirstName = member.FirstName,
                        LastName = member.LastName,
                        PhoneNumber = member.PhoneNumber,
                        Street = member.UserAddress.Street,
                        StreetNumber = member.UserAddress.StreetNumber,
                        City = member.UserAddress.City,
                        ZipCode = member.UserAddress.ZipCode
                    }, transaction);
                    await connection.ExecuteAsync(sqlMember, member, transaction);
                    transaction.Commit();
                }
            }
            return member;
        }

        public async Task<bool> DeleteMember(string SSN)
        {
            string sql = "DELETE FROM [User] WHERE SSN = @SSN";

            using (var connection = _connectionFactory.CreateConnection())
            {
                int rowsAffected = await connection.ExecuteAsync(sql, new { SSN });

                return rowsAffected != 0;
            }
        }
    }
}
