using DataAccess.DAO.DAOIntefaces;
using DataAccess.Models;
using DataAccess.Repositories.RepositoryInterfaces;
using Dapper;

namespace DataAccess.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly IDatabaseConnectionFactory _connectionFactory;

        public BookRepository(IDatabaseConnectionFactory databaseConnectionFactory)
        {
            _connectionFactory = databaseConnectionFactory;
        }

        public async Task<Book> GetBook(string ISBN)
        {
            string sql = "SELECT * FROM [Book] WHERE [ISBN] = @ISBN";
            Book book = new();

            using (var connection = _connectionFactory.CreateConnection())
            {
                book = await connection.QuerySingleAsync<Book>(sql, new
                {
                    ISBN
                });
            }
            return book;
        }

        public async Task<List<Book>> ListBooks()
        {
            string sql = "SELECT * FROM [Book]";
            List<Book> books = new();

            using (var connection = _connectionFactory.CreateConnection())
            {
                books = (await connection.QueryAsync<Book>(sql)).AsQueryable().ToList();
            }
            return books;
        }

        public async Task<Book> CreateBook(Book book)
        {
            string sql = "INSERT INTO [Book] (ISBN, CanLoan, Description, SubjectArea)" +
                " VALUES (@ISBN, @CanLoan, @Description, @SubjectArea)";

            using (var connection = _connectionFactory.CreateConnection())
            {
                var rowsAffected = await connection.ExecuteAsync(sql, book);
            }
            return book;
        }

        public async Task<Book> UpdateBook(Book book)
        {
            string sql = "UPDATE [Book] SET " +
                "[Name] = @Name, [CanLoan] = @CanLoan, [Description] = @Description " +
                "WHERE [ISBN] = @ISBN;";

            using (var connection = _connectionFactory.CreateConnection())
            {
                var rowsAffected = await connection.ExecuteAsync(sql, book);
            }
            return book;
        }

        public async Task<bool> DeleteBook(string ISBN)
        {
            string sql = "DELETE FROM Book WHERE ISBN = @ISBN";
            using (var connection = _connectionFactory.CreateConnection())
            {
                int rowsAffected = await connection.ExecuteAsync(sql, new
                {
                    ISBN
                });

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
