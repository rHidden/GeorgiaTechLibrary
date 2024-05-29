using DataAccess.DAO.DAOIntefaces;
using DataAccess.Models;
using DataAccess.Repositories.RepositoryInterfaces;
using Dapper;

namespace DataAccess.Repositories
{
    public class BookInstanceRepository : IBookInstanceRepository
    {
        private readonly IDatabaseConnectionFactory _connectionFactory;

        public BookInstanceRepository(IDatabaseConnectionFactory databaseConnectionFactory)
        {
            _connectionFactory = databaseConnectionFactory;
        }

        public async Task<BookInstance?> GetBookInstance(int id)
        {
            string sql = "SELECT bi.*, b.*, ba.Name " +
                "FROM BookInstance bi " +
                "LEFT JOIN Book b ON bi.BookISBN = b.ISBN " +
                "LEFT JOIN BookAuthor ba ON b.ISBN = ba.BookISBN " +
                "WHERE bi.Id = @Id";

            List<string> authors = new();

            using (var connection = _connectionFactory.CreateConnection())
            {
                var bookInstance = (await connection.QueryAsync<BookInstance, Book, string, BookInstance>(sql,
                    map: (bi, book, author) =>
                    {
                        authors.Add(author);
                        book.Authors = authors;
                        bi.Book = book;
                        return bi;
                    },
                    param: new { Id = id },
                    splitOn: "ISBN, Name"
                    )).FirstOrDefault();
                return bookInstance;
            }
        }

        public async Task<List<BookInstance>?> ListBookInstances()
        {
            string sql = "SELECT bi.*, b.*, ba.Name " +
                "FROM BookInstance bi " +
                "LEFT JOIN Book b ON bi.BookISBN = b.ISBN " +
                "LEFT JOIN BookAuthor ba ON b.ISBN = ba.BookISBN";

            Dictionary<int, List<string>> bookAuthorPairs = new();

            using (var connection = _connectionFactory.CreateConnection())
            {
                var bookInstances = (await connection.QueryAsync<BookInstance, Book, string, BookInstance>(sql,
                    map: (bi, book, author) =>
                    {
                        bookAuthorPairs.TryAdd(bi.Id, new List<string>());
                        var authors = bookAuthorPairs[bi.Id];
                        authors.Add(author);
                        book.Authors = authors;
                        bi.Book = book;
                        return bi;
                    },
                    splitOn: "ISBN, Name"
                    )).DistinctBy(book => book.Id).ToList();
                return bookInstances;
            }
        }

        public async Task<BookInstance> CreateBookInstance(BookInstance bookInstance)
        {
            string sqlInstance = "INSERT INTO [BookInstance] (Id, IsLoaned, BookISBN)" +
                " VALUES (@Id, @IsLoaned, @BookISBN)";

            using (var connection = _connectionFactory.CreateConnection())
            {
                await connection.ExecuteAsync(sqlInstance, new 
                { 
                    bookInstance.Id, 
                    bookInstance.IsLoaned, 
                    BookISBN = bookInstance.Book?.ISBN 
                });
            }
            return bookInstance;
        }

        public async Task<BookInstance> UpdateBookInstance(BookInstance bookInstance)
        {
            string sql = "UPDATE [BookInstance] SET " +
                "[IsLoaned] = @IsLoaned WHERE [Id] = @Id";

            using (var connection = _connectionFactory.CreateConnection())
            {
                var rowsAffected = await connection.ExecuteAsync(sql, new
                {
                    bookInstance.IsLoaned,
                    bookInstance.Id
                });
            }
            return bookInstance;
        }

        public async Task<bool> DeleteBookInstance(int id)
        {
            string sql = "DELETE FROM BookInstance WHERE Id = @Id";
            using (var connection = _connectionFactory.CreateConnection())
            {
                int rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });

                return rowsAffected == 1;
            }
        }
    }
}
