using Dapper;
using DataAccess.DAO.DAOIntefaces;
using DataAccess.Models;
using DataAccess.Repositories.RepositoryInterfaces;
using System.Transactions;

namespace DataAccess.Repositories
{
    public class LoanRepository : ILoanRepository
    {
        private readonly IDatabaseConnectionFactory _connectionFactory;

        public LoanRepository(IDatabaseConnectionFactory databaseConnectionFactory)
        {
            _connectionFactory = databaseConnectionFactory;
        }

        public async Task<Loan?> GetLoan(int id)
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

                        if (loan.User != null)
                        {
                            loan.User.SSN = user?.SSN;
                            loan.User.PhoneNumber = user?.PhoneNumber;
                            loan.User.FirstName = user?.FirstName;
                            loan.User.LastName = user?.LastName;
                            loan.User.UserAddress = address;
                        }

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

        public async Task<Loan?> CreateLoan(BookLoan loan)
        {
            string sql = @"
                DECLARE @InsertedIds TABLE (Id INT);
                INSERT INTO Loan (UserSSN, LoanDate, DueDate, LoanType, BookInstanceId) 
                OUTPUT Inserted.Id INTO @InsertedIds(Id)
                VALUES (@UserSSN, @LoanDate, @DueDate, @LoanType, @BookInstanceId);
                SELECT Id FROM @InsertedIds;";

            using (var connection = _connectionFactory.CreateConnection())
            {
                loan.Id = await connection.QuerySingleAsync<int>(sql, new
                {
                    UserSSN = loan.User?.SSN,
                    loan.LoanDate,
                    loan.DueDate,
                    LoanType = "Book",
                    BookInstanceId = loan.BookInstance?.Id
                });
                return loan;
            }
        }

        public async Task<Loan?> CreateLoan(DigitalItemLoan loan)
        {
            string sql = "INSERT INTO Loan (UserSSN, LoanDate, DueDate, LoanType, DigitalItemId) " +
                "OUTPUT Inserted.Id " +
                "VALUES (@UserSSN, @LoanDate, @DueDate, @LoanType, @DigitalItemId)";
            using (var connection = _connectionFactory.CreateConnection())
            {
                loan.Id = await connection.QuerySingleAsync<int>(sql, new
                {
                    UserSSN = loan.User?.SSN,
                    loan.LoanDate,
                    loan.DueDate,
                    LoanType = "DigitalItem",
                    DigitalItemId = loan.DigitalItem?.Id
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

        public async Task<Loan?> ReturnLoan(int id)
        {
            string getReservedUser = "SELECT u.*, b.*, br.ReservationDate FROM Loan l " +
                "INNER JOIN BookInstance bi ON bi.Id = l.BookInstanceId " +
                "INNER JOIN BookReservation br ON br.BookISBN = bi.BookISBN " +
                "INNER JOIN [User] u ON u.SSN = br.UserSSN " +
                "INNER JOIN Book b ON b.ISBN = br.BookISBN " +
                "WHERE l.Id = @Id " +
                "ORDER BY br.ReservationDate ASC";
            string getBookInstance = "SELECT bi.* FROM Loan l " +
                "INNER JOIN BookInstance bi ON bi.Id = l.BookInstanceId " +
                "WHERE l.Id = @Id";
            string deleteReservation = "DELETE FROM BookReservation " +
                "WHERE UserSSN = @UserSSN AND BookISBN = @BookISBN AND ReservationDate = @ReservationDate";
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                using (var connection = _connectionFactory.CreateConnection())
                {
                    //GetLoan
                    var loan = await GetLoan(id);
                    if (loan?.ReturnDate == null)
                    {
                        loan.ReturnDate = DateTime.Now;
                        //Return Loan
                        var updatedLoan = await UpdateLoan(loan);

                        //Check for book reservation
                        var bookReservation = (await connection.QueryAsync<User, Book, DateTime, BookReservation>(getReservedUser,
                            map: (user, book, reservationDate) =>
                            {
                                return new BookReservation(user, book, reservationDate);
                            },
                            param: new { id },
                            splitOn: "ISBN, ReservationDate")).FirstOrDefault();
                        if (bookReservation != null)
                        {
                            //Get book instance
                            var bookInstance = await connection.QuerySingleAsync<BookInstance>(getBookInstance,
                                new { id });
                            bookInstance.Book = bookReservation.Book;

                            //Make book loan for reservation
                            BookLoan newBookLoan = new();
                            newBookLoan.LoanDate = loan.ReturnDate;
                            newBookLoan.DueDate = loan.ReturnDate?.AddMonths(1);
                            newBookLoan.User = bookReservation.User;
                            newBookLoan.BookInstance = bookInstance;

                            //Create new loan for reservation
                            await CreateLoan(newBookLoan);

                            //Delete reservation
                            await connection.ExecuteAsync(deleteReservation,
                                new
                                {
                                    UserSSN = bookReservation.User?.SSN,
                                    BookISBN = bookReservation.Book?.ISBN,
                                    bookReservation.ReservationDate
                                });
                        }
                        scope.Complete();
                        return updatedLoan;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        public async Task<(double Books, double Videos, double Audios, double Texts, double Images)> GetLoanItemStatistics()
        {
            string sql = "SELECT CAST(COUNT(l.BookInstanceId) AS FLOAT) / COUNT(l.Id) * 100 AS Books, " +
                "CAST(SUM(CASE WHEN di.DigitalItemType = 'Video' THEN 1 ELSE 0 END) AS FLOAT) / COUNT(l.Id) * 100 AS Videos, " +
                "CAST(SUM(CASE WHEN di.DigitalItemType = 'Audio' THEN 1 ELSE 0 END) AS FLOAT) / COUNT(l.Id) * 100 AS Audios, " +
                "CAST(SUM(CASE WHEN di.DigitalItemType = 'Text' THEN 1 ELSE 0 END) AS FLOAT) / COUNT(l.Id) * 100 AS Texts, " +
                "CAST(SUM(CASE WHEN di.DigitalItemType = 'Image' THEN 1 ELSE 0 END) AS FLOAT) / COUNT(l.Id) * 100 AS Images " +
                "FROM Loan l LEFT JOIN DigitalItem di ON di.Id = l.DigitalItemId";
            using (var connection = _connectionFactory.CreateConnection())
            {
                var statistics = await connection.QuerySingleAsync<(double Books, double Videos, double Audios,
                    double Texts, double Images)>(sql);
                return statistics;
            }
        }

        public async Task<float> GetAverageNumberOfDaysToReturnBooks()
        {
            string sql = "SELECT dbo.GetAverageNumberOfDaysToReturnBooks();";
            using (var connection = _connectionFactory.CreateConnection())
            {
                await connection.OpenAsync();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    var result = await command.ExecuteScalarAsync();

                    if (result != null && float.TryParse(result.ToString(), out float averageNumberOfDays))
                    {
                        return averageNumberOfDays;
                    }
                    else
                    {
                        throw new Exception("Problem retrieving average return days");
                    }
                }
            }
        }

    }
}
