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
            string sql = "SELECT b.*, ba.Name " +
                "FROM Book b " +
                "LEFT JOIN BookAuthor ba ON b.ISBN = ba.BookISBN " +
                "WHERE ISBN = @ISBN";

            List<string> authors = new();

            using (var connection = _connectionFactory.CreateConnection())
            {
                var book = (await connection.QueryAsync<Book, string,Book>(sql, 
                    map: (book, author) =>
                    {
                        authors.Add(author);
                        book.Authors = authors;
                        return book;
                    },  
                    param: new { ISBN },
                    splitOn: "Name"
                    )).AsQueryable().FirstOrDefault();
                return book;
            }
        }

        public async Task<List<Book>> ListBooks()
        {
            string sql = "SELECT b.*, ba.Name " +
                "FROM Book b " +
                "LEFT JOIN BookAuthor ba ON b.ISBN = ba.BookISBN";

            Dictionary<string, List<string>> bookAuthorPairs = new();

            using (var connection = _connectionFactory.CreateConnection())
            {
                var books = (await connection.QueryAsync<Book, string, Book>(sql,
                    map: (book, author) =>
                    {
                        bookAuthorPairs.TryAdd(book.ISBN ?? "", new List<string>());
                        var authors = bookAuthorPairs[book.ISBN ?? ""];
                        authors.Add(author);
                        book.Authors = authors;
                        return book;
                    },
                    splitOn: "Name"
                    )).AsQueryable().DistinctBy(book => book.ISBN).ToList();
                return books;
            }
        }

        public async Task<Book> CreateBook(Book book)
        {
            string sqlBook = "INSERT INTO [Book] (ISBN, [Status], Description, SubjectArea)" +
                " VALUES (@ISBN, @Status, @Description, @SubjectArea)";
            string sqlAuthor = "INSERT INTO [BookAuthor] (BookISBN, Name)" +
                " VALUES (@BookISBN, @Name)";

            using (var connection = _connectionFactory.CreateConnection())
            {
                //using (var transaction = connection.BeginTransaction())
                //{
                    await connection.ExecuteAsync(sqlBook, book);
                    foreach (var author in book.Authors ?? [])
                    {
                        await connection.ExecuteAsync(sqlAuthor, new { BookISBN = book.ISBN, Name = author });
                    }
                    //transaction.Commit();
                //}
            }
            return book;
        }

        public async Task<Book> UpdateBook(Book book)
        {
            string sql = "UPDATE [Book] SET " +
                "[Name] = @Name, [Status] = @Status, [Description] = @Description " +
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

                return rowsAffected == 1;
            }
        }

        public async Task<List<Book>> GetMostPopularBooksAmongStudents()
        {
            string sql = "SELECT TOP 5 WITH TIES b.ISBN " +
                "FROM Book b " +
                "INNER JOIN BookInstance bi ON bi.BookISBN = b.ISBN " +
                "INNER JOIN Loan l ON l.BookInstanceId = bi.Id " +
                "INNER JOIN [Member] m ON m.UserSSN = l.UserSSN " +
                "WHERE m.MemberType = 'Student' " +
                "GROUP BY b.ISBN " +
                "ORDER BY COUNT(l.Id) DESC";
            using(var connection = _connectionFactory.CreateConnection())
            {
                var bookISBNs = (await connection.QueryAsync<string>(sql)).ToList();

                List<Book> books = new();

                foreach (var isbn in bookISBNs)
                {
                    books.Add(await GetBook(isbn));
                }

                return books;
            }
        }
    }

}
