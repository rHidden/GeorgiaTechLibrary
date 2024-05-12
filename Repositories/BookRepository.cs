using GeorgiaTechLibrary.Models;
using GeorgiaTechLibrary.Repositories.RepositoryInterfaces;
using Microsoft.Data.SqlClient;

namespace GeorgiaTechLibrary.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly IDatabaseConnectionFactory _connectionFactory;
        public BookRepository(IDatabaseConnectionFactory databaseConnectionFactory)
        {
            _connectionFactory = databaseConnectionFactory;
        }
        public async Task<Book> CreateBook(Book book)
        {
            using var con = _connectionFactory.CreateConnection();
            await con.OpenAsync();
            var sql = @"INSERT INTO Book (ISBN, CanLoan, Description, SubjectArea) VALUES
                (@ISBN, @CanLoan, @Description, @SubjectArea)";
            using (SqlCommand command = new SqlCommand(sql, con))
            {
                command.Parameters.AddWithValue("@ISBN", book.ISBN);
                command.Parameters.AddWithValue("@CanLoan", book.CanLoan);
                command.Parameters.AddWithValue("@Description", book.Description);
                command.Parameters.AddWithValue("@SubjectArea", book.SubjectArea);

                await command.ExecuteNonQueryAsync();
            };
            return book;
        }


        public async Task DeleteBook(int ISBN)
        {
            using var con = _connectionFactory.CreateConnection();
            await con.OpenAsync();

            using var transaction = con.BeginTransaction();
            var sql = @"DELETE FROM Book WHERE ISBN = @ISBN";

            using (SqlCommand command = new SqlCommand(sql, con, transaction))
            {
                try
                {
                    command.Parameters.AddWithValue("@ISBN", ISBN);
                    var affectedRows = await command.ExecuteNonQueryAsync();

                    if (affectedRows > 0)
                    {
                        transaction.Commit();
                        await Console.Out.WriteLineAsync($"Info: Book with ID {ISBN} was successfully deleted.");
                    }
                    else
                    {
                        transaction.Rollback();
                        await Console.Out.WriteLineAsync($"Warning: No book found with ID: {ISBN} - no changes were made.");
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    await Console.Out.WriteLineAsync($"Error: Failed to delete book with ID: {ISBN}. Error: {ex.Message}");
                }
            }
        }

        public async Task<Book> GetBook(int ISBN)
        {
            using var con = _connectionFactory.CreateConnection();
            await con.OpenAsync();
            var sql = @"SELECT * FROM Book WHERE ISBN = @ISBN";
            using (SqlCommand command = new SqlCommand(sql, con))
            {
                command.Parameters.AddWithValue("@ISBN", ISBN);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        var book = new Book
                        {
                            ISBN = reader.GetString(reader.GetOrdinal("ISBN")),
                            CanLoan = reader.GetBoolean(reader.GetOrdinal("CanLoan")),
                            Description = reader.GetString(reader.GetOrdinal("Description")),
                            SubjectArea = reader.GetString(reader.GetOrdinal("SubjectArea")),
                        };
                        await Console.Out.WriteLineAsync($"Info: Book with ISBN {ISBN} was found.");
                        return book;
                    }
                    else
                    {
                        await Console.Out.WriteLineAsync($"Warning: Book with ISBN {ISBN} was not found.");
                        return null;
                    }
                }
            };
        }

        public async Task<List<Book>> ListBooks()
        {
            using var con = _connectionFactory.CreateConnection();
            await con.OpenAsync();
            var sql = @"SELECT * FROM Book";

            using (SqlCommand command = new SqlCommand(sql, con))
            {
                using (var reader = await command.ExecuteReaderAsync())
                {
                    var books = new List<Book>();

                    while (await reader.ReadAsync())
                    {
                        var book = new Book
                        {
                            ISBN = reader.GetString(reader.GetOrdinal("ISBN")),
                            CanLoan = reader.GetBoolean(reader.GetOrdinal("CanLoan")),
                            Description = reader.GetString(reader.GetOrdinal("Description")),
                            SubjectArea = reader.GetString(reader.GetOrdinal("SubjectArea")),
                        };

                        books.Add(book);
                    }

                    if (books.Count > 0)
                    {
                        await Console.Out.WriteLineAsync($"Info: Found {books.Count} books.");
                    }
                    else
                    {
                        await Console.Out.WriteLineAsync("Warning: No books were found.");
                    }

                    return books;
                }
            }
        }


        public async Task UpdateBook(Book book)
        {
            using var con = _connectionFactory.CreateConnection();
            await con.OpenAsync();

            using var transaction = con.BeginTransaction();
            var sql = @"UPDATE Book SET Description = @Description, SubjectArea = @SubjectArea WHERE ISBN = @ISBN";

            using (SqlCommand command = new SqlCommand(sql, con, transaction))
            {
                try
                {
                    // Prevention against SQL injection
                    command.Parameters.AddWithValue("@ISBN", book.ISBN);
                    command.Parameters.AddWithValue("@Description", book.Description ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@SubjectArea", book.SubjectArea ?? (object)DBNull.Value);

                    var affectedRows = await command.ExecuteNonQueryAsync();

                    if (affectedRows > 0)
                    {
                        transaction.Commit();
                        await Console.Out.WriteLineAsync($"Info: Book with ISBN: {book.ISBN} was successfully updated.");
                    }
                    else
                    {
                        transaction.Rollback();
                        await Console.Out.WriteLineAsync($"Warning: Book with ISBN: {book.ISBN} was not found - no changes were made.");
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    await Console.Out.WriteLineAsync($"Error: Failed to update book with ISBN {book.ISBN}. Error: {ex.Message}");
                    throw;
                }
            }
        }
    }
}
