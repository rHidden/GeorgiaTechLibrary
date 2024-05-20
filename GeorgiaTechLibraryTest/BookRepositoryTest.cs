using Xunit;
using DataAccess.DAO;
using DataAccess.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using Moq;
using DataAccess.Repositories;
using DataAccess.DAO.DAOIntefaces;
using DataAccess.Repositories.RepositoryInterfaces;
using System.Threading.Tasks;

namespace DataAccessTest
{
    public class BookRepositoryTest
    {
        private Mock<IDatabaseConnectionFactory> _mockDatabaseConnectionFactory;
        private Mock<IBookRepository> _mockBookRepository;
        private string _connectionString;

        public BookRepositoryTest()
        {
            _connectionString = DatabaseConnectionTest._connectionString;
            _mockDatabaseConnectionFactory = new Mock<IDatabaseConnectionFactory>();
            _mockDatabaseConnectionFactory.Setup(d => d.CreateConnection())
               .Returns(new SqlConnection(_connectionString));
            _mockBookRepository = new Mock<IBookRepository>();
        }

        [Fact]
        public async Task CreateBook_CreatesNewBook()
        {
            // Arrange
            var newBook = new Book { ISBN = "3456789012345", CanLoan = true, Description = "New Test Book", SubjectArea = "History" };

            _mockBookRepository.Setup(r => r.CreateBook(newBook)).ReturnsAsync(newBook);

            // Act
            var result = await _mockBookRepository.Object.CreateBook(newBook);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(newBook.ISBN, result.ISBN);
            Assert.True(result.CanLoan);
            Assert.Equal(newBook.Description, result.Description);
            Assert.Equal(newBook.SubjectArea, result.SubjectArea);
        }

        [Fact]
        public async Task GetBook_WithValidISBN_ReturnsBook()
        {
            // Arrange
            var isbn = "1234567890123";
            var expectedBook = new Book { ISBN = isbn, CanLoan = true, Description = "Test Book", SubjectArea = "Fiction" };

            _mockBookRepository.Setup(r => r.GetBook(isbn)).ReturnsAsync(expectedBook);

            // Act
            var result = await _mockBookRepository.Object.GetBook(isbn);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedBook.ISBN, result.ISBN);
            Assert.True(result.CanLoan);
            Assert.Equal(expectedBook.Description, result.Description);
            Assert.Equal(expectedBook.SubjectArea, result.SubjectArea);
        }

        [Fact]
        public async Task ListBooks_ReturnsListOfBooks()
        {
            // Arrange
            var book1 = new Book { ISBN = "1234567890123", CanLoan = true, Description = "Test Book 1", SubjectArea = "Fiction" };
            var book2 = new Book { ISBN = "2345678901234", CanLoan = false, Description = "Test Book 2", SubjectArea = "Non-Fiction" };
            var books = new List<Book> { book1, book2 };

            _mockBookRepository.Setup(r => r.ListBooks()).ReturnsAsync(books);

            // Act
            var result = await _mockBookRepository.Object.ListBooks();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(book1, result);
            Assert.Contains(book2, result);
        }

        [Fact]
        public async Task UpdateBook_UpdatesExistingBook()
        {
            // Arrange
            var isbn = "1234567890123";
            var originalBook = new Book { ISBN = isbn, CanLoan = true, Description = "Test Book", SubjectArea = "Fiction" };
            var updatedBook = new Book { ISBN = isbn, CanLoan = false, Description = "Updated Test Book", SubjectArea = "Science Fiction" };

            _mockBookRepository.Setup(r => r.UpdateBook(updatedBook)).ReturnsAsync(updatedBook);

            // Act
            var result = await _mockBookRepository.Object.UpdateBook(updatedBook);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(isbn, result.ISBN);
            Assert.False(result.CanLoan);
            Assert.Equal(updatedBook.Description, result.Description);
            Assert.Equal(updatedBook.SubjectArea, result.SubjectArea);
        }

        [Fact]
        public async Task DeleteBook_RemovesBookFromDatabase()
        {
            // Arrange
            var isbn = "1234567890123";
            var book = new Book { ISBN = isbn, CanLoan = true, Description = "Test Book", SubjectArea = "Fiction" };

            _mockBookRepository.Setup(r => r.DeleteBook(isbn)).ReturnsAsync(book);
            _mockBookRepository.Setup(r => r.GetBook(isbn)).ReturnsAsync((Book)null);

            // Act
            var deletedBook = await _mockBookRepository.Object.DeleteBook(isbn);
            var result = await _mockBookRepository.Object.GetBook(isbn);

            // Assert
            Assert.NotNull(deletedBook);
            Assert.Equal(isbn, deletedBook.ISBN);
            Assert.Null(result);
        }
    }
}
