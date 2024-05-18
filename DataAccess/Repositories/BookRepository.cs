using DataAccess.DAO.DAOIntefaces;
using DataAccess.Models;
using DataAccess.Repositories.RepositoryInterfaces;
using System.Data.Common;

namespace DataAccess.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly IDatabaseConnectionFactory _connectionFactory;

        public BookRepository(IDatabaseConnectionFactory databaseConnectionFactory)
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

        public async Task<Book> GetBook(string ISBN)
        {
            try
            {
                using (var connection = _connectionFactory.CreateConnection())
                {
                    await connection.OpenAsync();
                    var command = connection.CreateCommand();
                    command.CommandText = "SELECT * FROM Book WHERE ISBN = @ISBN";
                    AddParameter(command, "@ISBN", ISBN);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new Book
                            {
                                ISBN = reader["ISBN"].ToString(),
                                CanLoan = (bool)reader["CanLoan"],
                                Description = reader["Description"].ToString(),
                                SubjectArea = reader["SubjectArea"].ToString()
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving book: {ex.Message}");
            }
            return null;
        }

        public async Task<List<Book>> ListBooks()
        {
            var books = new List<Book>();
            try
            {
                using (var connection = _connectionFactory.CreateConnection())
                {
                    await connection.OpenAsync();
                    var command = connection.CreateCommand();
                    command.CommandText = "SELECT * FROM Book";

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            books.Add(new Book
                            {
                                ISBN = reader["ISBN"].ToString(),
                                CanLoan = (bool)reader["CanLoan"],
                                Description = reader["Description"].ToString(),
                                SubjectArea = reader["SubjectArea"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error listing books: {ex.Message}");
            }
            return books;
        }

        public async Task<Book> CreateBook(Book book)
        {
            try
            {
                using (var connection = _connectionFactory.CreateConnection())
                {
                    await connection.OpenAsync();
                    var command = connection.CreateCommand();
                    command.CommandText = "INSERT INTO Book (ISBN, CanLoan, Description, SubjectArea) VALUES (@ISBN, @CanLoan, @Description, @SubjectArea)";

                    AddParameter(command, "@ISBN", book.ISBN);
                    AddParameter(command, "@CanLoan", book.CanLoan);
                    AddParameter(command, "@Description", book.Description);
                    AddParameter(command, "@SubjectArea", book.SubjectArea);

                    await command.ExecuteNonQueryAsync();
                    return book;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating book: {ex.Message}");
            }
            return null;
        }

        public async Task UpdateBook(Book book)
        {
            try
            {
                using (var connection = _connectionFactory.CreateConnection())
                {
                    await connection.OpenAsync();
                    var command = connection.CreateCommand();

                    var updateFields = new List<string>();
                    if (book.CanLoan != null)
                    {
                        updateFields.Add("CanLoan = @CanLoan");
                        AddParameter(command, "@CanLoan", book.CanLoan);
                    }

                    if (!string.IsNullOrEmpty(book.Description))
                    {
                        updateFields.Add("Description = @Description");
                        AddParameter(command, "@Description", book.Description);
                    }

                    if (!string.IsNullOrEmpty(book.SubjectArea))
                    {
                        updateFields.Add("SubjectArea = @SubjectArea");
                        AddParameter(command, "@SubjectArea", book.SubjectArea);
                    }

                    if (updateFields.Count == 0)
                    {
                        Console.WriteLine("No fields to update");
                        return;
                    }

                    command.CommandText = $"UPDATE Book SET {string.Join(", ", updateFields)} WHERE ISBN = @ISBN";
                    AddParameter(command, "@ISBN", book.ISBN);

                    await command.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating book: {ex.Message}");
            }
        }

        public async Task<Book> DeleteBook(string ISBN)
        {
            try
            {
                using (var connection = _connectionFactory.CreateConnection())
                {
                    await connection.OpenAsync();
                    var command = connection.CreateCommand();
                    command.CommandText = "DELETE FROM Book WHERE ISBN = @ISBN";

                    AddParameter(command, "@ISBN", ISBN);

                    var book = await GetBook(ISBN);

                    await command.ExecuteNonQueryAsync();
                    return book;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting book: {ex.Message}");
            }
            return null;
        }
    }

}
