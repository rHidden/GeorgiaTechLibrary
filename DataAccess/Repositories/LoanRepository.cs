using DataAccess.DAO.DAOIntefaces;
using DataAccess.Models;
using DataAccess.Repositories.RepositoryInterfaces;
using System.Data.Common;

namespace DataAccess.Repositories
{
    public class LoanRepository : ILoanRepository
    {
        private readonly IDatabaseConnectionFactory _connectionFactory;

        public LoanRepository(IDatabaseConnectionFactory databaseConnectionFactory)
        {
            _connectionFactory = databaseConnectionFactory;
        }

        private void AddParameter(DbCommand command, string name, object value)
        {
            var param = command.CreateParameter();
            param.ParameterName = name;
            param.Value = value ?? DBNull.Value;
            command.Parameters.Add(param);
        }

        public async Task<Loan> GetLoan(int id)
        {
            try
            {
                using (var connection = _connectionFactory.CreateConnection())
                {
                    await connection.OpenAsync();
                    var command = connection.CreateCommand();
                    command.CommandText = "SELECT * FROM Loan WHERE Id = @Id";
                    AddParameter(command, "@Id", id);

                    Loan loan = null;
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            loan = new Loan
                            {
                                Id = (int)reader["Id"],
                                LoanDate = reader["LoanDate"] != DBNull.Value ? (DateTime)reader["LoanDate"] : null,
                                DueDate = reader["DueDate"] != DBNull.Value ? (DateTime)reader["DueDate"] : null,
                                ReturnDate = reader["ReturnDate"] != DBNull.Value ? (DateTime)reader["ReturnDate"] : null,
                                Type = reader["DigitalItemId"] != DBNull.Value ? LoanType.DigitalItem : LoanType.Book,
                                DigitalItem = reader["DigitalItemId"] != DBNull.Value ? new DigitalItem { Id = (int)reader["DigitalItemId"] } : null,
                                BookInstance = reader["BookInstanceId"] != DBNull.Value ? new BookInstance { Id = (int)reader["BookInstanceId"] } : null,
                                User = new User { SSN = reader["UserSSN"].ToString() }
                            };
                        }
                    }

                    if (loan != null)
                    {
                        // Fetch User details
                        command.CommandText = "SELECT * FROM [User] WHERE SSN = @UserSSN";
                        command.Parameters.Clear();
                        AddParameter(command, "@UserSSN", loan.User.SSN);
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                loan.User.FirstName = reader["FirstName"].ToString();
                                loan.User.LastName = reader["LastName"].ToString();
                                loan.User.PhoneNumber = reader["PhoneNumber"].ToString();
                                loan.User.UserAddress = new Address
                                {
                                    Street = reader["Street"].ToString(),
                                    StreetNumber = reader["StreetNumber"].ToString(),
                                    City = reader["City"].ToString(),
                                    ZipCode = reader["Zipcode"].ToString()
                                };
                            }
                        }

                        // Fetch Item details based on type
                        if (loan.Type == LoanType.Book)
                        {
                            var bookInstance = (BookInstance)loan.BookInstance;
                            command.CommandText = @"SELECT *
                                                    FROM BookInstance bi
                                                    JOIN Book b ON bi.BookISBN = b.ISBN
                                                    WHERE bi.Id = @ItemId";
                            command.Parameters.Clear();
                            AddParameter(command, "@ItemId", bookInstance.Id);
                            using (var reader = await command.ExecuteReaderAsync())
                            {
                                if (await reader.ReadAsync())
                                {
                                    bookInstance.Book = new Book
                                    {
                                        ISBN = reader["BookISBN"].ToString(),
                                        CanLoan = (bool)reader["CanLoan"],
                                        SubjectArea = reader["SubjectArea"].ToString(),
                                        Description = reader["Description"].ToString(),
                                    };
                                    bookInstance.IsLoaned = (bool)reader["IsLoaned"];
                                }
                            }
                        }
                        else if (loan.Type == LoanType.DigitalItem)
                        {
                            var digitalItem = (DigitalItem)loan.DigitalItem;
                            command.CommandText = "SELECT * FROM DigitalItem WHERE Id = @ItemId";
                            command.Parameters.Clear();
                            AddParameter(command, "@ItemId", digitalItem.Id);
                            using (var reader = await command.ExecuteReaderAsync())
                            {
                                if (await reader.ReadAsync())
                                {
                                    digitalItem.Name = reader["Name"].ToString();
                                    digitalItem.Author = reader["Author"].ToString();
                                    digitalItem.Format = reader["Format"].ToString();
                                    digitalItem.Size = double.Parse(reader["Size"].ToString());
                                }
                            }
                        }
                    }

                    return loan;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving loan: {ex.Message}");
            }
            return null;
        }

        public async Task<List<Loan>> ListUserLoans(string userSSN)
        {
            var loans = new List<Loan>();
            try
            {
                using (var connection = _connectionFactory.CreateConnection())
                {
                    await connection.OpenAsync();
                    var command = connection.CreateCommand();
                    command.CommandText = "SELECT * FROM Loan WHERE UserSSN = @UserSSN";
                    AddParameter(command, "@UserSSN", userSSN);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var loan = await GetLoan((int)reader["Id"]);
                            loans.Add(loan);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error listing user loans: {ex.Message}");
            }
            return loans;
        }

        public async Task<Loan> CreateLoan(Loan loan) //TODO cant loan not canLoan book
        {
            try
            {
                using (var connection = _connectionFactory.CreateConnection())
                {
                    await connection.OpenAsync();
                    var command = connection.CreateCommand();
                    command.CommandText = "INSERT INTO Loan (Id, LoanDate, DueDate, UserSSN, DigitalItemId, BookInstanceId) VALUES (@Id, @LoanDate, @DueDate, @UserSSN, @DigitalItemId, @BookInstanceId)";

                    AddParameter(command, "@Id", loan.Id);
                    AddParameter(command, "@LoanDate", loan.LoanDate);
                    AddParameter(command, "@DueDate", loan.DueDate);
                    AddParameter(command, "@UserSSN", loan.User.SSN);
                    AddParameter(command, "@DigitalItemId", loan.Type == LoanType.DigitalItem ? loan.DigitalItem.Id : (object)DBNull.Value);
                    AddParameter(command, "@BookInstanceId", loan.Type == LoanType.Book ? loan.BookInstance.Id : (object)DBNull.Value);

                    await command.ExecuteNonQueryAsync();
                    return loan;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating loan: {ex.Message}");
            }
            return null;
        }

        public async Task<Loan> UpdateLoan(Loan loan)
        {
            try
            {
                using (var connection = _connectionFactory.CreateConnection())
                {
                    await connection.OpenAsync();
                    var command = connection.CreateCommand();

                    var query = "UPDATE Loan SET ";
                    var parameters = new List<string>();

                    if (loan.DueDate.HasValue)
                    {
                        parameters.Add("DueDate = @DueDate");
                        AddParameter(command, "@DueDate", loan.DueDate.Value);
                    }
                    if (loan.ReturnDate.HasValue)
                    {
                        parameters.Add("ReturnDate = @ReturnDate");
                        AddParameter(command, "@ReturnDate", loan.ReturnDate.Value);
                    }

                    if (parameters.Count == 0)
                    {
                        Console.WriteLine("No fields to update");
                        return null;
                    }

                    query += string.Join(", ", parameters) + " WHERE Id = @Id";
                    command.CommandText = query;
                    AddParameter(command, "@Id", loan.Id);

                    await command.ExecuteNonQueryAsync();
                    return await GetLoan(loan.Id);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating loan: {ex.Message}");
                return null;
            }
        }

        public async Task<Loan> DeleteLoan(int id)
        {
            try
            {
                var loan = await GetLoan(id);

                if (loan == null)
                {
                    Console.WriteLine("Loan not found");
                    return null;
                }

                using (var connection = _connectionFactory.CreateConnection())
                {
                    await connection.OpenAsync();
                    var command = connection.CreateCommand();
                    command.CommandText = "DELETE FROM Loan WHERE Id = @Id";

                    AddParameter(command, "@Id", id);

                    await command.ExecuteNonQueryAsync();
                }

                return loan;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting loan: {ex.Message}");
                return null;
            }
        }
    }
}
