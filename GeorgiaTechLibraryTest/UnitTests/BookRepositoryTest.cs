using DataAccess.Models;
using Microsoft.Data.SqlClient;
using Moq;
using DataAccess.Repositories;
using DataAccess.DAO.DAOIntefaces;
using static DataAccess.Models.Book;

namespace GeorgiaTechLibraryTest.UnitTests
{
    public class BookRepositoryTest
    {
        private Mock<IDatabaseConnectionFactory> _mockDatabaseConnectionFactory;
        private BookRepository _bookRepository;
        private string _connectionString;

        public BookRepositoryTest()
        {
            _connectionString = DatabaseConnectionTest._connectionString;
            _mockDatabaseConnectionFactory = new Mock<IDatabaseConnectionFactory>();
            _mockDatabaseConnectionFactory.Setup(d => d.CreateConnection())
                .Returns(() => new SqlConnection(_connectionString));
            _bookRepository = new BookRepository(_mockDatabaseConnectionFactory.Object);
        }

        [Fact]
        public async Task CreateBook_CreatesNewBook()
        {
            // Arrange
            var newBook = new Book { ISBN = "13", Name = "Test book", Status = BookStatus.loanable, Description = "New Test Book", SubjectArea = "History" };

            // Act
            var result = await _bookRepository.CreateBook(newBook);
            await _bookRepository.DeleteBook(newBook.ISBN);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(newBook.ISBN, result.ISBN);
            Assert.Equal(result.Status, BookStatus.loanable);
            Assert.Equal(newBook.Description, result.Description);
            Assert.Equal(newBook.SubjectArea, result.SubjectArea);
        }

        [Fact]
        public async Task GetBook_WithValidISBN_ReturnsBook()
        {
            // Arrange
            var expectedBook = new Book { ISBN = "14", Name = "Test book", Status = BookStatus.loanable, Description = "Test Book", SubjectArea = "Fiction" };

            // Act
            await _bookRepository.CreateBook(expectedBook);
            var result = await _bookRepository.GetBook(expectedBook.ISBN);
            await _bookRepository.DeleteBook(expectedBook.ISBN);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedBook.ISBN, result.ISBN);
            Assert.Equal(result.Status, BookStatus.loanable);
            Assert.Equal(expectedBook.Description, result.Description);
            Assert.Equal(expectedBook.SubjectArea, result.SubjectArea);
        }

        [Fact]
        public async Task ListBooks_ReturnsListOfBooks()
        {
            // Arrange
            var book1 = new Book { ISBN = "15", Status = BookStatus.loanable, Description = "Test Book 1", SubjectArea = "Fiction" };
            var book2 = new Book { ISBN = "16", Status = BookStatus.unloanable, Description = "Test Book 2", SubjectArea = "Non-Fiction" };
            var books = new List<Book> { book1, book2 };

            // Act
            await _bookRepository.CreateBook(book1);
            await _bookRepository.CreateBook(book2);
            var result = await _bookRepository.ListBooks();
            await _bookRepository.DeleteBook(book1.ISBN);
            await _bookRepository.DeleteBook(book2.ISBN);

            // Assert
            Assert.Contains(result, r => r.ISBN == book1.ISBN && r.Description == book1.Description && r.Status == book1.Status && r.SubjectArea == book1.SubjectArea);
            Assert.Contains(result, r => r.ISBN == book2.ISBN && r.Description == book2.Description && r.Status == book2.Status && r.SubjectArea == book2.SubjectArea);
        }

        [Fact]
        public async Task UpdateBook_UpdatesExistingBook()
        {
            // Arrange
            var originalBook = new Book { ISBN = "17", Name = "Test book", Status = BookStatus.loanable, Description = "Test Book", SubjectArea = "Fiction" };
            var updatedBook = new Book { ISBN = "17", Name = "Test book", Status = BookStatus.unloanable, Description = "Updated Test Book", SubjectArea = "Science Fiction" };

            // Act
            await _bookRepository.CreateBook(originalBook);
            var result = await _bookRepository.UpdateBook(updatedBook);
            await _bookRepository.DeleteBook(updatedBook.ISBN);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(updatedBook.ISBN, result.ISBN);
            Assert.Equal(result.Status, BookStatus.unloanable);
            Assert.Equal(updatedBook.Description, result.Description);
            Assert.Equal(updatedBook.SubjectArea, result.SubjectArea);
        }

        [Fact]
        public async Task DeleteBook_RemovesBookFromDatabase()
        {
            // Arrange
            var book = new Book { ISBN = "18", Name = "Test book", Status = BookStatus.loanable, Description = "Test Book", SubjectArea = "Fiction" };

            // Act
            await _bookRepository.CreateBook(book);
            var deletedBook = await _bookRepository.DeleteBook(book.ISBN);

            // Assert
            Assert.NotNull(deletedBook);
        }
    }
}
