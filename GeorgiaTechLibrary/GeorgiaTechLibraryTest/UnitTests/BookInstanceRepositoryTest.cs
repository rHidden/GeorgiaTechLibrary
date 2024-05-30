using DataAccess.Models;
using Microsoft.Data.SqlClient;
using Moq;
using DataAccess.Repositories;
using DataAccess.DAO.DAOIntefaces;

namespace GeorgiaTechLibraryTest.UnitTests
{
    public class BookInstanceRepositoryTest
    {
        private Mock<IDatabaseConnectionFactory> _mockDatabaseConnectionFactory;
        private BookInstanceRepository _bookInstanceRepository;
        private BookRepository _bookRepository;
        private string _connectionString;

        public BookInstanceRepositoryTest()
        {
            _connectionString = DatabaseConnectionTest._connectionString;
            _mockDatabaseConnectionFactory = new Mock<IDatabaseConnectionFactory>();
            _mockDatabaseConnectionFactory.Setup(d => d.CreateConnection())
              .Returns(() => new SqlConnection(_connectionString));
            _bookInstanceRepository = new BookInstanceRepository(_mockDatabaseConnectionFactory.Object);
            _bookRepository = new BookRepository(_mockDatabaseConnectionFactory.Object);
        }

        [Fact]
        public async Task GetBookInstance_ValidId_ReturnsBookInstance()
        {
            // Arrange
            var book = new Book
            {
                ISBN = "1",
                Authors = new List<string> { "Author Name" }
            };

            var expectedBookInstance = new BookInstance
            {
                Id = 1,
                IsLoaned = false,
                Book = book
            };

            // Act
            await _bookRepository.CreateBook(book);
            await _bookInstanceRepository.CreateBookInstance(expectedBookInstance);
            var result = await _bookInstanceRepository.GetBookInstance(expectedBookInstance.Id);
            await _bookInstanceRepository.DeleteBookInstance(expectedBookInstance.Id);
            await _bookRepository.DeleteBook(book.ISBN);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedBookInstance.Id, result.Id);
            Assert.Equal(expectedBookInstance.IsLoaned, result.IsLoaned);
            Assert.Equal(expectedBookInstance.Book.ISBN, result.Book.ISBN);
            Assert.Equal(expectedBookInstance.Book.Authors, result.Book.Authors);
        }

        [Fact]
        public async Task ListBookInstances_ReturnsListofBookInstances()
        {
            // Arrange
            var book1 = new Book
            {
                ISBN = "2",
                Authors = new List<string> { "Author Name" }
            };

            var book2 = new Book
            {
                ISBN = "3",
                Authors = new List<string> { "Another Author" }
            };

            var expectedBookInstance1 = new BookInstance
            {
                Id = 2,
                IsLoaned = false,
                Book = book1
            };

            var expectedBookInstance2 = new BookInstance
            {
                Id = 3,
                IsLoaned = true,
                Book = book2
            };

            var bookInstances = new List<BookInstance> { expectedBookInstance1, expectedBookInstance2 };

            // Act
            await _bookRepository.CreateBook(book1);
            await _bookRepository.CreateBook(book2);
            await _bookInstanceRepository.CreateBookInstance(expectedBookInstance1);
            await _bookInstanceRepository.CreateBookInstance(expectedBookInstance2);
            var result = await _bookInstanceRepository.ListBookInstances();
            await _bookInstanceRepository.DeleteBookInstance(expectedBookInstance1.Id);
            await _bookInstanceRepository.DeleteBookInstance(expectedBookInstance2.Id);
            await _bookRepository.DeleteBook(book1.ISBN);
            await _bookRepository.DeleteBook(book2.ISBN);

            // Assert
            var actualBookInstance1 = result.FirstOrDefault(r => r.Id == expectedBookInstance1.Id);
            var actualBookInstance2 = result.FirstOrDefault(r => r.Id == expectedBookInstance2.Id);

            Assert.NotNull(actualBookInstance1);
            Assert.NotNull(actualBookInstance2);

            Assert.Equal(expectedBookInstance1.IsLoaned, actualBookInstance1.IsLoaned);
            Assert.Equal(expectedBookInstance1.Book.ISBN, actualBookInstance1.Book.ISBN);
            Assert.Equal(expectedBookInstance1.Book.Authors.OrderBy(a => a), actualBookInstance1.Book.Authors.OrderBy(a => a));

            Assert.Equal(expectedBookInstance2.IsLoaned, actualBookInstance2.IsLoaned);
            Assert.Equal(expectedBookInstance2.Book.ISBN, actualBookInstance2.Book.ISBN);
            Assert.Equal(expectedBookInstance2.Book.Authors.OrderBy(a => a), actualBookInstance2.Book.Authors.OrderBy(a => a));
        }


        [Fact]
        public async Task CreateBookInstance_CreatesNewBookInstance()
        {
            // Arrange
            var book = new Book
            {
                ISBN = "4",
                Authors = new List<string> { "Yet Another Author" }
            };

            var newBookInstance = new BookInstance
            {
                Id = 4,
                IsLoaned = false,
                Book = book
            };

            // Act
            await _bookRepository.CreateBook(book);
            var result = await _bookInstanceRepository.CreateBookInstance(newBookInstance);
            await _bookInstanceRepository.DeleteBookInstance(newBookInstance.Id);
            await _bookRepository.DeleteBook(book.ISBN);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(newBookInstance.Id, result.Id);
            Assert.Equal(newBookInstance.IsLoaned, result.IsLoaned);
            Assert.Equal(newBookInstance.Book.ISBN, result.Book.ISBN);
            Assert.Equal(newBookInstance.Book.Authors, result.Book.Authors);
        }

        [Fact]
        public async Task UpdateBookInstance_UpdatesExistingBookInstance()
        {
            // Arrange
            var book = new Book
            {
                ISBN = "5",
                Authors = new List<string> { "Updated Author" }
            };

            var updatedBookInstance = new BookInstance
            {
                Id = 5,
                IsLoaned = true,
                Book = book
            };

            // Act
            await _bookRepository.CreateBook(book);
            await _bookInstanceRepository.CreateBookInstance(updatedBookInstance);
            var result = await _bookInstanceRepository.UpdateBookInstance(updatedBookInstance);
            await _bookInstanceRepository.DeleteBookInstance(updatedBookInstance.Id);
            await _bookRepository.DeleteBook(book.ISBN);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(updatedBookInstance.Id, result.Id);
            Assert.Equal(updatedBookInstance.IsLoaned, result.IsLoaned);
            Assert.Equal(updatedBookInstance.Book.ISBN, result.Book.ISBN);
            Assert.Equal(updatedBookInstance.Book.Authors, result.Book.Authors);
        }

        [Fact]
        public async Task DeleteBookInstance_RemovesBookInstanceFromDatabase()
        {
            // Arrange
            var book = new Book
            {
                ISBN = "6",
                Authors = new List<string> { "Updated Author" }
            };

            var deletedBookInstance = new BookInstance
            {
                Id = 6,
                IsLoaned = true,
                Book = book
            };

            // Act
            await _bookRepository.CreateBook(book);
            await _bookInstanceRepository.CreateBookInstance(deletedBookInstance);
            var result = await _bookInstanceRepository.DeleteBookInstance(deletedBookInstance.Id);
            await _bookRepository.DeleteBook(book.ISBN);

            // Assert
            Assert.True(result);
        }
    }
}
