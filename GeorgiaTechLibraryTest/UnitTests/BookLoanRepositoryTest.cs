using DataAccess.Models;
using Microsoft.Data.SqlClient;
using Moq;
using DataAccess.Repositories;
using DataAccess.DAO.DAOIntefaces;

namespace GeorgiaTechLibraryTest.UnitTests
{
    public class BookLoanRepositoryTest
    {
        private Mock<IDatabaseConnectionFactory> _mockDatabaseConnectionFactory;
        private LoanRepository _loanRepository;
        private BookRepository _bookRepository;
        private MemberRepository _memberRepository;
        private BookInstanceRepository _bookInstanceRepository;
        private string _connectionString;

        public BookLoanRepositoryTest()
        {
            _connectionString = DatabaseConnectionTest._connectionString;
            _mockDatabaseConnectionFactory = new Mock<IDatabaseConnectionFactory>();
            _mockDatabaseConnectionFactory.Setup(d => d.CreateConnection())
                .Returns(() => new SqlConnection(_connectionString));
            _loanRepository = new LoanRepository(_mockDatabaseConnectionFactory.Object);
            _bookRepository = new BookRepository(_mockDatabaseConnectionFactory.Object);
            _memberRepository = new MemberRepository(_mockDatabaseConnectionFactory.Object);
            _bookInstanceRepository = new BookInstanceRepository(_mockDatabaseConnectionFactory.Object);
        }

        [Fact]
        public async Task CreateBookLoan_CreatesNewBookLoan()
        {
            // Arrange
            var user = new Member { UserAddress = new Address { Street = "Main St", StreetNumber = "1", City = "City", ZipCode = "12345" }, SSN = "123456785", CardNumber = "123456" };

            var book = new Book
            {
                Description = "Test Book",
                Authors = new List<string> { "Author1" },
                ISBN = "7",
                CanLoan = true
            };

            var bookInstance = new BookInstance
            {
                Id = 7,
                IsLoaned = false,
                Book = book
            };

            var newBookLoan = new BookLoan
            {
                LoanDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(30),
                ReturnDate = null,
                User = user,
                BookInstance = bookInstance
            };

            // Act
            await _memberRepository.CreateMember(user);
            await _bookRepository.CreateBook(book);
            await _bookInstanceRepository.CreateBookInstance(bookInstance);
            var result = await _loanRepository.CreateLoan(newBookLoan);
            await _loanRepository.DeleteLoan(result.Id);
            await _bookInstanceRepository.DeleteBookInstance(bookInstance.Id);
            await _bookRepository.DeleteBook(book.ISBN);
            await _memberRepository.DeleteMember(user.SSN);

            // Assert
            var createdBookLoan = Assert.IsType<BookLoan>(result);
            Assert.NotNull(result);
            Assert.Equal(newBookLoan.LoanDate, createdBookLoan.LoanDate);
            Assert.Equal(newBookLoan.DueDate, createdBookLoan.DueDate);
            Assert.Null(createdBookLoan.ReturnDate);
            Assert.Equal(newBookLoan.User.SSN, createdBookLoan.User.SSN);
            Assert.Equal(newBookLoan.BookInstance.Id, createdBookLoan.BookInstance.Id);
        }

        [Fact]
        public async Task GetBookLoan_WithValidId_ReturnsBookLoan()
        {
            // Arrange
            var user = new Member { UserAddress = new Address { Street = "Main St", StreetNumber = "1", City = "City", ZipCode = "12345" }, SSN = "123456784", CardNumber = "123456" };

            var book = new Book
            {
                Description = "Test Book",
                Authors = new List<string> { "Author1" },
                ISBN = "8",
                CanLoan = true
            };

            var bookInstance = new BookInstance
            {
                Id = 8,
                IsLoaned = false,
                Book = book
            };

            var expectedBookLoan = new BookLoan
            {
                LoanDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(30),
                ReturnDate = null,
                User = user,
                BookInstance = bookInstance
            };

            // Act
            await _memberRepository.CreateMember(user);
            await _bookRepository.CreateBook(book);
            await _bookInstanceRepository.CreateBookInstance(bookInstance);
            await _loanRepository.CreateLoan(expectedBookLoan);
            var result = await _loanRepository.GetLoan(expectedBookLoan.Id);
            await _loanRepository.DeleteLoan(expectedBookLoan.Id);
            await _bookInstanceRepository.DeleteBookInstance(bookInstance.Id);
            await _bookRepository.DeleteBook(book.ISBN);
            await _memberRepository.DeleteMember(user.SSN);

            // Assert
            var retrievedBookLoan = Assert.IsType<BookLoan>(result);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task ListUserBookLoans_WithValidSSN_ReturnsListOfBookLoans()
        {
            // Arrange
            var user = new Member { UserAddress = new Address { Street = "Main St", StreetNumber = "1", City = "City", ZipCode = "12345" }, SSN = "123456783", CardNumber = "123456" };

            var book1 = new Book
            {
                Description = "Test Book 1",
                Authors = new List<string> { "Author1" },
                ISBN = "9",
                CanLoan = true
            };

            var book2 = new Book
            {
                Description = "Test Book 2",
                Authors = new List<string> { "Author1" },
                ISBN = "10",
                CanLoan = true
            };

            var bookInstance1 = new BookInstance
            {
                Id = 9,
                IsLoaned = false,
                Book = book1
            };

            var bookInstance2 = new BookInstance
            {
                Id = 10,
                IsLoaned = false,
                Book = book2
            };

            var bookLoan1 = new BookLoan
            {
                LoanDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(30),
                ReturnDate = null,
                User = user,
                BookInstance = bookInstance1
            };

            var bookLoan2 = new BookLoan
            {
                LoanDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(30),
                ReturnDate = null,
                User = user,
                BookInstance = bookInstance2
            };

            // Act
            await _memberRepository.CreateMember(user);
            await _bookRepository.CreateBook(book1);
            await _bookRepository.CreateBook(book2);
            await _bookInstanceRepository.CreateBookInstance(bookInstance1);
            await _bookInstanceRepository.CreateBookInstance(bookInstance2);
            await _loanRepository.CreateLoan(bookLoan1);
            await _loanRepository.CreateLoan(bookLoan2);
            var result = await _loanRepository.ListUserLoans(user.SSN);
            await _loanRepository.DeleteLoan(bookLoan1.Id);
            await _loanRepository.DeleteLoan(bookLoan2.Id);
            await _bookInstanceRepository.DeleteBookInstance(bookInstance1.Id);
            await _bookInstanceRepository.DeleteBookInstance(bookInstance2.Id);
            await _bookRepository.DeleteBook(book1.ISBN);
            await _bookRepository.DeleteBook(book2.ISBN);
            await _memberRepository.DeleteMember(user.SSN);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, r => r.Id == bookLoan1.Id && r is BookLoan);
            Assert.Contains(result, r => r.Id == bookLoan2.Id && r is BookLoan);
        }

        [Fact]
        public async Task UpdateBookLoan_UpdatesExistingBookLoan()
        {
            // Arrange
            var user = new Member { UserAddress = new Address { Street = "Main St", StreetNumber = "1", City = "City", ZipCode = "12345" }, SSN = "123456782", CardNumber = "123456" };

            var book = new Book
            {
                Description = "Test Book",
                Authors = new List<string> { "Author1" },
                ISBN = "11",
                CanLoan = true
            };

            var bookInstance = new BookInstance
            {
                Id = 11,
                IsLoaned = false,
                Book = book
            };

            var originalBookLoan = new BookLoan
            {
                LoanDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(30),
                ReturnDate = null,
                User = user,
                BookInstance = bookInstance
            };

            var updatedBookLoan = new BookLoan
            {
                LoanDate = DateTime.Now.AddDays(-1),
                DueDate = DateTime.Now.AddDays(29),
                ReturnDate = DateTime.Now,
                User = user,
                BookInstance = bookInstance
            };

            // Act
            await _memberRepository.CreateMember(user);
            await _bookRepository.CreateBook(book);
            await _bookInstanceRepository.CreateBookInstance(bookInstance);
            await _loanRepository.CreateLoan(originalBookLoan);
            var result = await _loanRepository.UpdateLoan(updatedBookLoan);
            await _loanRepository.DeleteLoan(updatedBookLoan.Id);
            await _bookInstanceRepository.DeleteBookInstance(bookInstance.Id);
            await _bookRepository.DeleteBook(book.ISBN);
            await _memberRepository.DeleteMember(user.SSN);

            // Assert
            var updatedLoan = Assert.IsType<BookLoan>(result);
            Assert.NotNull(result);
            Assert.Equal(updatedBookLoan.LoanDate, updatedLoan.LoanDate);
            Assert.Equal(updatedBookLoan.DueDate, updatedLoan.DueDate);
            Assert.Equal(updatedBookLoan.ReturnDate, updatedLoan.ReturnDate);
            Assert.Equal(updatedBookLoan.User.SSN, updatedLoan.User.SSN);
            Assert.Equal(updatedBookLoan.BookInstance.Id, updatedLoan.BookInstance.Id);
        }

        [Fact]
        public async Task DeleteBookLoan_DeletesExistingBookLoan()
        {
            // Arrange
            var user = new Member { UserAddress = new Address { Street = "Main St", StreetNumber = "1", City = "City", ZipCode = "12345" }, SSN = "123456781", CardNumber = "123456" };

            var book = new Book
            {
                Description = "Test Book",
                Authors = new List<string> { "Author1" },
                ISBN = "12",
                CanLoan = true
            };

            var bookInstance = new BookInstance
            {
                Id = 12,
                IsLoaned = false,
                Book = book
            };

            var bookLoan = new BookLoan
            {
                LoanDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(30),
                ReturnDate = null,
                User = user,
                BookInstance = bookInstance
            };

            // Act
            await _memberRepository.CreateMember(user);
            await _bookRepository.CreateBook(book);
            await _bookInstanceRepository.CreateBookInstance(bookInstance);
            await _loanRepository.CreateLoan(bookLoan);
            var result = await _loanRepository.DeleteLoan(bookLoan.Id);
            await _bookInstanceRepository.DeleteBookInstance(bookInstance.Id);
            await _bookRepository.DeleteBook(book.ISBN);
            await _memberRepository.DeleteMember(user.SSN);

            // Assert
            Assert.True(result);
        }
    }
}
