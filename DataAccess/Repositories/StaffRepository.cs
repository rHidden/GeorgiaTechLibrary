using DataAccess.Repositories.RepositoryInterfaces;
using DataAccess.Models;
using Dapper;
using DataAccess.DAO.DAOIntefaces;

namespace DataAccess.Repositories
{
    public class StaffRepository : IStaffRepository
    {
        private readonly IDatabaseConnectionFactory _connectionFactory;

        public StaffRepository(IDatabaseConnectionFactory databaseConnectionFactory)
        {
            _connectionFactory = databaseConnectionFactory;
        }

        public async Task<Staff> GetStaff(string SSN)
        {
            string sql = @"
                SELECT u.SSN, u.PhoneNumber, u.FirstName, u.LastName, u.Street, u.StreetNumber, u.City, u.Zipcode, s.LibrarianNumber, s.Role
                FROM [User] u
                INNER JOIN [Staff] s ON u.SSN = s.UserSSN
                WHERE u.SSN = @SSN";

            using (var connection = _connectionFactory.CreateConnection())
            {
                var staff = (await connection.QueryAsync<User, Address, Staff, Staff>(sql, (user, address, staff) =>
                {
                    staff.SSN = user.SSN;
                    staff.FirstName = user.FirstName;
                    staff.LastName = user.LastName;
                    staff.PhoneNumber = user.PhoneNumber;
                    staff.UserAddress = address;
                    return staff;
                }, new { SSN }, splitOn: "Street, LibrarianNumber")).AsQueryable().FirstOrDefault();

                return staff;
            }
        }

        public async Task<List<Staff>> ListStaff()
        {
            string sql = @"
                SELECT u.SSN, u.PhoneNumber, u.FirstName, u.LastName, u.Street, u.StreetNumber, u.City, u.Zipcode, s.LibrarianNumber, s.Role
                FROM [User] u
                INNER JOIN [Staff] s ON u.SSN = s.UserSSN";

            using (var connection = _connectionFactory.CreateConnection())
            {
                var staffMembers = await connection.QueryAsync<User, Address, Staff, Staff>(sql, (user, address, staff) =>
                {
                    staff.SSN = user.SSN;
                    staff.FirstName = user.FirstName;
                    staff.LastName = user.LastName;
                    staff.PhoneNumber = user.PhoneNumber;
                    staff.UserAddress = address;
                    return staff;
                }, splitOn: "Street, LibrarianNumber");

                return staffMembers.ToList();
            }
        }

        public async Task<Staff> CreateStaff(Staff staff)
        {
            string sqlUser = "INSERT INTO [User] (SSN, FirstName, LastName, PhoneNumber, Street, StreetNumber, City, Zipcode) " +
                "VALUES (@SSN, @FirstName, @LastName, @PhoneNumber, @Street, @StreetNumber, @City, @Zipcode)";

            string sqlStaff = "INSERT INTO [Staff] (UserSSN, LibrarianNumber, Role) " +
                "VALUES (@SSN, @LibrarianNumber, @Role)";

            using (var connection = _connectionFactory.CreateConnection())
            {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction())
                {
                    await connection.ExecuteAsync(sqlUser, new
                    {
                        SSN = staff.SSN,
                        FirstName = staff.FirstName,
                        LastName = staff.LastName,
                        PhoneNumber = staff.PhoneNumber,
                        Street = staff.UserAddress.Street,
                        StreetNumber = staff.UserAddress.StreetNumber,
                        City = staff.UserAddress.City,
                        ZipCode = staff.UserAddress.ZipCode
                    }, transaction);
                    await connection.ExecuteAsync(sqlStaff, new
                    {
                        SSN = staff.SSN,
                        LibrarianNumber = staff.LibrarianNumber,
                        Role = staff.Role
                    }, transaction);
                    transaction.Commit();
                }
            }
            return staff;
        }

        public async Task<Staff> UpdateStaff(Staff staff)
        {
            string sqlUser = "UPDATE [User] SET " +
                "FirstName = @FirstName, LastName = @LastName, PhoneNumber = @PhoneNumber, Street = @Street, StreetNumber = @StreetNumber, City = @City, Zipcode = @Zipcode " +
                "WHERE SSN = @SSN";

            string sqlStaff = "UPDATE [Staff] SET " +
                "LibrarianNumber = @LibrarianNumber, Role = @Role " +
                "WHERE UserSSN = @SSN";

            using (var connection = _connectionFactory.CreateConnection())
            {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction())
                {
                    await connection.ExecuteAsync(sqlUser, new
                    {
                        SSN = staff.SSN,
                        FirstName = staff.FirstName,
                        LastName = staff.LastName,
                        PhoneNumber = staff.PhoneNumber,
                        Street = staff.UserAddress.Street,
                        StreetNumber = staff.UserAddress.StreetNumber,
                        City = staff.UserAddress.City,
                        ZipCode = staff.UserAddress.ZipCode
                    }, transaction);
                    await connection.ExecuteAsync(sqlStaff, new
                    {
                        SSN = staff.SSN,
                        LibrarianNumber = staff.LibrarianNumber,
                        Role = staff.Role
                    }, transaction);
                    transaction.Commit();
                }
            }
            return staff;
        }

        public async Task<bool> DeleteStaff(string SSN)
        {
            string sql = "DELETE FROM [User] WHERE SSN = @SSN";

            using (var connection = _connectionFactory.CreateConnection())
            {
                int rowsAffected = await connection.ExecuteAsync(sql, new { SSN });

                return rowsAffected == 1;
            }
        }
    }
}
