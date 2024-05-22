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
        private BookRepository _bookRepository;
        private string _connectionString;

        public BookRepositoryTest()
        {
            _connectionString = DatabaseConnectionTest._connectionString;
            _mockDatabaseConnectionFactory = new Mock<IDatabaseConnectionFactory>();
            _mockDatabaseConnectionFactory.Setup(d => d.CreateConnection())
               .Returns(new SqlConnection(_connectionString));
            _bookRepository = new BookRepository(_mockDatabaseConnectionFactory.Object);
        }

        [Fact]
        public async Task CreateBook_CreatesNewBook()
        {
            // Arrange
            var isbn =  "3456789012345";
            var newBook = new Book { ISBN = isbn, Name = "Test book", CanLoan = true, Description = "New Test Book", SubjectArea = "History" };

            // Act
            var result = await _bookRepository.CreateBook(newBook);
            await _bookRepository.DeleteBook(isbn);

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
            var expectedBook = new Book { ISBN = isbn, Name = "Test book", CanLoan = true, Description = "Test Book", SubjectArea = "Fiction" };

            // Act
            await _bookRepository.CreateBook(expectedBook);
            var result = await _bookRepository.GetBook(isbn);
            await _bookRepository.DeleteBook(isbn);

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
            var isbn1 = "1234567890123";
            var isbn2 = "2345678901234";
            var book1 = new Book { ISBN = "1234567890123", Name = "Test book", CanLoan = true, Description = "Test Book 1", SubjectArea = "Fiction" };
            var book2 = new Book { ISBN = "2345678901234", Name = "Test book", CanLoan = false, Description = "Test Book 2", SubjectArea = "Non-Fiction" };
            var books = new List<Book> { book1, book2 };

            // Act
            await _bookRepository.CreateBook(book1);
            await _bookRepository.CreateBook(book2);
            var result = await _bookRepository.ListBooks();
            await _bookRepository.DeleteBook(isbn1);
            await _bookRepository.DeleteBook(isbn2);

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
            var originalBook = new Book { ISBN = isbn, Name = "Test book", CanLoan = true, Description = "Test Book", SubjectArea = "Fiction" };
            var updatedBook = new Book { ISBN = isbn, Name = "Test book", CanLoan = false, Description = "Updated Test Book", SubjectArea = "Science Fiction" };

            // Act
            await _bookRepository.CreateBook(originalBook);
            var result = await _bookRepository.UpdateBook(updatedBook);
            await _bookRepository.DeleteBook(isbn);

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
            var book = new Book { ISBN = isbn, Name = "Test book", CanLoan = true, Description = "Test Book", SubjectArea = "Fiction" };

            // Act
            await _bookRepository.CreateBook(book);
            var deletedBook = await _bookRepository.DeleteBook(isbn);
            var result = await _bookRepository.GetBook(isbn);

            // Assert
            Assert.NotNull(deletedBook);
            Assert.Null(result);
        }
    }
}
