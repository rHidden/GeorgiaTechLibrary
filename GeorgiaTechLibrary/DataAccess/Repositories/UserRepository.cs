using DataAccess.Repositories.RepositoryInterfaces;
using DataAccess.Models;
using Dapper;
using DataAccess.DAO.DAOIntefaces;
using System.Runtime.Intrinsics.X86;
using System.Collections.Generic;
using System.Reflection;

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

        public async Task<List<(User, int SumOfDaysOfBeingLate)>> GetDaysLate()
        {
            string sql = "SELECT u.SSN, SUM(DATEDIFF(day, l.DueDate, l.ReturnDate)) AS SumOfDaysOfBeingLate " +
                "FROM [User] u " +
                "INNER JOIN Loan l ON l.UserSSN = u.SSN " +
                "WHERE l.LoanDate > DATEADD(YEAR, -1, GETDATE()) " +
                "AND l.ReturnDate > l.DueDate " +
                "GROUP BY u.SSN " +
                "HAVING COUNT(DATEDIFF(day, l.DueDate, l.ReturnDate)) > 2 " +
                "ORDER BY SumOfDaysOfBeingLate DESC";
            using (var connection = _connectionFactory.CreateConnection())
            {
                var lateUserSSNs = (await connection.QueryAsync<string, int, (string UserSSN, int SumOfDaysOfBeingLate)>(sql, map: (userSSN, daysLate) =>
                {
                    return (userSSN, daysLate);
                },
                splitOn: "SumOfDaysOfBeingLate"
                )).ToList();

                var userTasks = lateUserSSNs.Select(async x => (await GetUser(x.UserSSN) ?? new User(), x.SumOfDaysOfBeingLate));
                var userResults = await Task.WhenAll(userTasks);
                var lateUsers = userResults.ToList();

                return lateUsers;
            }
        }

        public async Task<List<(User, int AvgLoanDuration)>> GetAverageLoanDuration()
        {
            string sql = "SELECT u.SSN, AVG(DATEDIFF(day, l.LoanDate, l.ReturnDate)) AS AvgLoanDuration " +
                "FROM [User] u " +
                "INNER JOIN Loan l ON l.UserSSN = u.SSN " +
                "WHERE l.LoanType = 'Book' " +
                "AND l.ReturnDate IS NOT NULL " +
                "GROUP BY u.SSN " +
                "ORDER BY AvgLoanDuration DESC, u.SSN";
            using (var connection = _connectionFactory.CreateConnection())
            {
                var avgLoanDurationSSNs = (await connection.QueryAsync<string, int, (string UserSSN, int AvgLoanDuration)>(sql, map: (userSSN, daysLate) =>
                {
                    return (userSSN, daysLate);
                },
                splitOn: "AvgLoanDuration"
                )).ToList();

                var userTasks = avgLoanDurationSSNs.Select(async x => (await GetUser(x.UserSSN) ?? new User(), x.AvgLoanDuration));
                var userResults = await Task.WhenAll(userTasks);
                var avgLoanDuration = userResults.ToList();

                return avgLoanDuration;
            }
        }

        private async Task<User?> GetUser(string SSN)
        {
            string sql = "SELECT SSN, PhoneNumber, FirstName, " +
                "LastName, Street, StreetNumber, City, Zipcode " +
                "FROM [User] " +
                "WHERE SSN = @SSN";
            using (var connection = _connectionFactory.CreateConnection())
            {
                var users = (await connection.QueryAsync<User, Address, User>(sql, map: (user, address) =>
                {
                    user.UserAddress = address;
                    return user;
                },
                param: new { SSN },
                splitOn: "Street"
                )).FirstOrDefault();
                return users;
            }
        }
    }
}
