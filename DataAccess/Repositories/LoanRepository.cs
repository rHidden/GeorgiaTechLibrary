using Dapper;
using DataAccess.DAO.DAOIntefaces;
using DataAccess.Models;
using DataAccess.Repositories.RepositoryInterfaces;

namespace DataAccess.Repositories
{
    public class LoanRepository : ILoanRepository
    {
        private readonly IDatabaseConnectionFactory _connectionFactory;

        public LoanRepository(IDatabaseConnectionFactory databaseConnectionFactory)
        {
            _connectionFactory = databaseConnectionFactory;
        }

        public async Task<Loan> GetLoan(int id)
        {
            string sql = "SELECT loan.Id, loan.LoanDate, loan.ReturnDate, loan.DueDate, " +
                "[user].SSN, [user].FirstName, [user].LastName, [user].PhoneNumber, [user].Street, " +
                "[user].StreetNumber, [user].City, [user].Zipcode, staff.[Role], staff.LibrarianNumber, " +
                "[member].CardNumber, [member].ExpiryDate, [member].Photo, [member].MemberType, " +
                "bookInstance.Id, bookInstance.IsLoaned, digitalItem.[Name], digitalItem.Id, digitalItem.[Format], " +
                "digitalItem.Size " +
                "FROM Loan loan " +
                "INNER JOIN [User] as [user] on [user].SSN = loan.UserSSN " +
                "LEFT JOIN Staff staff on staff.UserSSN = [user].SSN " +
                "LEFT JOIN [Member] [member] on [member].UserSSN = [user].SSN " +
                "LEFT JOIN BookInstance bookInstance on bookInstance.Id = loan.BookInstanceId " +
                "LEFT JOIN DigitalItemLibrary digitalItemLibrary on digitalItemLibrary.DigitalItemId = loan.DigitalItemId " +
                "LEFT JOIN DigitalItem digitalItem on digitalItem.Id = digitalItemLibrary.DigitalItemId " +
                "WHERE loan.Id = @Id";
            using (var connection = _connectionFactory.CreateConnection())
            {
                var loan = (await connection.QueryAsync<Loan, Staff, Address, Staff, Member,
                    BookInstance, DigitalItem, Loan>(sql, 
                    map: (loan, user, address, staff, member, bookInstance, digitalItem) =>
                    {
                        if(staff != null)
                        {
                            loan.User = staff;
                        } 
                        else
                        {
                            loan.User = member;
                        }

                        loan.User.SSN = user.SSN;
                        loan.User.PhoneNumber = user.PhoneNumber;
                        loan.User.FirstName = user.FirstName;
                        loan.User.LastName = user.LastName;
                        loan.User.UserAddress = address;

                        if(bookInstance != null) 
                        {
                            loan = new BookLoan(loan, bookInstance);
                        }
                        else
                        {
                            loan = new DigitalItemLoan(loan, digitalItem);
                        }
                        return loan;
                    }, 
                    param: new { id }, 
                    splitOn: "SSN, Street, Role, CardNumber, Id, Name")
                    ).AsQueryable().FirstOrDefault();
                return loan;
            }
        }

        public async Task<List<Loan>> ListUserLoans(string userSSN)
        {
            string sql = "SELECT loan.Id, loan.LoanDate, loan.ReturnDate, loan.DueDate, " +
                "[user].SSN, [user].FirstName, [user].LastName, [user].PhoneNumber, [user].Street, " +
                "[user].StreetNumber, [user].City, [user].Zipcode, staff.[Role], staff.LibrarianNumber, " +
                "[member].CardNumber, [member].ExpiryDate, [member].Photo, [member].MemberType, " +
                "bookInstance.Id, bookInstance.IsLoaned, digitalItem.[Name], digitalItem.Id, digitalItem.[Format], " +
                "digitalItem.Size " +
                "FROM Loan loan " +
                "INNER JOIN [User] as [user] on [user].SSN = loan.UserSSN " +
                "LEFT JOIN Staff staff on staff.UserSSN = [user].SSN " +
                "LEFT JOIN [Member] [member] on [member].UserSSN = [user].SSN " +
                "LEFT JOIN BookInstance bookInstance on bookInstance.Id = loan.BookInstanceId " +
                "LEFT JOIN DigitalItemLibrary digitalItemLibrary on digitalItemLibrary.DigitalItemId = loan.DigitalItemId " +
                "LEFT JOIN DigitalItem digitalItem on digitalItem.Id = digitalItemLibrary.DigitalItemId " +
                "WHERE [user].SSN = @SSN";
            using (var connection = _connectionFactory.CreateConnection())
            {
                var loan = (await connection.QueryAsync<Loan, User, Address, Staff, Member,
                    BookInstance, DigitalItem, Loan>(sql,
                    map: (loan, user, address, staff, member, bookInstance, digitalItem) =>
                    {
                        if (staff != null)
                        {
                            loan.User = staff;
                        }
                        else
                        {
                            loan.User = member;
                        }

                        loan.User.SSN = user.SSN;
                        loan.User.PhoneNumber = user.PhoneNumber;
                        loan.User.FirstName = user.FirstName;
                        loan.User.LastName = user.LastName;
                        loan.User.UserAddress = address;

                        if (bookInstance != null)
                        {
                            loan = new BookLoan(loan, bookInstance);
                        }
                        else
                        {
                            loan = new DigitalItemLoan(loan, digitalItem);
                        }
                        return loan;
                    }, 
                    param: new { SSN = userSSN },
                    splitOn: "SSN, Street, Role, CardNumber, Id, Name")
                    ).AsQueryable().AsList();
                return loan;
            }
        }

        public async Task<Loan?> CreateLoan(BookLoan loan) //TODO cant loan not canLoan book
        {
            if (loan.BookInstance?.Book?.CanLoan ?? false)
            {
                string sql = "INSERT INTO Loan (Id, UserSSN, LoanDate, ReturnDate, DueDate, LoanType, BookInstanceId) " +
                    "VALUES (@Id, @UserSSN, @LoanDate, @ReturnDate, @DueDate, @LoanType, @BookInstanceId)";
                using (var connection = _connectionFactory.CreateConnection())
                {
                    var rowsAffected = await connection.ExecuteAsync(sql, new
                    {
                        loan.Id,
                        UserSSN = loan.User?.SSN,
                        loan.LoanDate,
                        loan.ReturnDate,
                        loan.DueDate,
                        LoanType = "Book",
                        BookInstanceId = loan.BookInstance?.Id
                    });
                    return loan;
                }
            }
            else
            {
                return null;
            }
        }

        public async Task<Loan> CreateLoan(DigitalItemLoan loan) //TODO cant loan not canLoan book
        {
            string sql = "INSERT INTO Loan (Id, UserSSN, LoanDate, ReturnDate, DueDate, LoanType, DigitalItemId) " +
                "VALUES (@Id, @UserSSN, @LoanDate, @ReturnDate, @DueDate, @LoanType, @DigitalItemId)";
            using (var connection = _connectionFactory.CreateConnection())
            {
                var rowsAffected = await connection.ExecuteAsync(sql, new
                {
                    loan.Id,
                    UserSSN = loan.User?.SSN,
                    loan.LoanDate,
                    loan.ReturnDate,
                    loan.DueDate,
                    LoanType = "DigitalItem",
                    DIgitalItemId = loan.DigitalItem?.Id
                });
                return loan;
            }
        }

        public async Task<Loan> UpdateLoan(Loan loan)
        {
            string sql = "UPDATE Loan SET ReturnDate = @ReturnDate, DueDate = @DueDate " +
                "WHERE Id = @Id";
            using (var connection = _connectionFactory.CreateConnection())
            {
                var rowsAffected = await connection.ExecuteAsync(sql, new
                {
                    UserSSN = loan.User?.SSN,
                    loan.ReturnDate,
                    loan.DueDate,
                    loan.Id
                });
                return loan;
            }
        }

        public async Task<bool> DeleteLoan(int id)
        {
            string sql = "DELETE FROM Loan WHERE Id = @Id";
            using (var connection = _connectionFactory.CreateConnection())
            {
                int rowsAffected = await connection.ExecuteAsync(sql, new { id });

                if (rowsAffected != 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
