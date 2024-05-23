//using Xunit;
//using DataAccess.DAO;
//using DataAccess.Models;
//using Microsoft.Data.SqlClient;
//using System;
//using System.Collections.Generic;
//using Moq;
//using DataAccess.Repositories;
//using DataAccess.DAO.DAOIntefaces;
//using DataAccess.Repositories.RepositoryInterfaces;
//using System.Threading.Tasks;
//using DataAccessTest;

//namespace GeorgiaTechLibraryTest
//{
//    public class BookLoanRepositoryTest
//    {
//        private Mock<IDatabaseConnectionFactory> _mockDatabaseConnectionFactory;
//        private LoanRepository _loanRepository;
//        private string _connectionString;

//        public BookLoanRepositoryTest()
//        {
//            _connectionString = DatabaseConnectionTest._connectionString;
//            _mockDatabaseConnectionFactory = new Mock<IDatabaseConnectionFactory>();
//            _mockDatabaseConnectionFactory.Setup(d => d.CreateConnection())
//                .Returns(() => new SqlConnection(_connectionString));
//            _loanRepository = new LoanRepository(_mockDatabaseConnectionFactory.Object);
//        }

//        [Fact]
//        public async Task CreateBookLoan_CreatesNewBookLoan()
//        {
//            // Arrange
//            var newBookLoan = new BookLoan
//            {
//                LoanDate = DateTime.Now,
//                DueDate = DateTime.Now.AddDays(30),
//                ReturnDate = null,
//                User = new User { SSN = "123-45-6789" },
//                BookInstance = new BookInstance { Id = 123, IsLoaned = true, Book = new Book() }
//            };

//            // Act
//            var result = await _loanRepository.CreateLoan(newBookLoan);
//            await _loanRepository.DeleteLoan(result.Id);

//            // Assert
//            var createdBookLoan = Assert.IsType<BookLoan>(result);
//            Assert.NotNull(result);
//            Assert.Equal(newBookLoan.LoanDate, createdBookLoan.LoanDate);
//            Assert.Equal(newBookLoan.DueDate, createdBookLoan.DueDate);
//            Assert.Null(createdBookLoan.ReturnDate);
//            Assert.Equal(newBookLoan.User.SSN, createdBookLoan.User.SSN);
//            Assert.Equal(newBookLoan.BookInstance.Id, createdBookLoan.BookInstance.Id);
//        }

//        [Fact]
//        public async Task GetBookLoan_WithValidId_ReturnsBookLoan()
//        {
//            // Arrange
//            var bookLoanId = 1;
//            var expectedBookLoan = new BookLoan
//            {
//                LoanDate = DateTime.Now,
//                DueDate = DateTime.Now.AddDays(30),
//                ReturnDate = null,
//                User = new User { SSN = "123-45-6789" },
//                BookInstance = new BookInstance { Id = 123, IsLoaned = true, Book = new Book() }
//            };

//            // Act
//            await _loanRepository.CreateLoan(expectedBookLoan);
//            var result = await _loanRepository.GetLoan(bookLoanId);
//            await _loanRepository.DeleteLoan(bookLoanId);

//            // Assert
//            var retrievedBookLoan = Assert.IsType<BookLoan>(result);
//            Assert.NotNull(result);
//            Assert.Equal(expectedBookLoan.LoanDate, retrievedBookLoan.LoanDate);
//            Assert.Equal(expectedBookLoan.DueDate, retrievedBookLoan.DueDate);
//            Assert.Null(retrievedBookLoan.ReturnDate);
//            Assert.Equal(expectedBookLoan.User.SSN, retrievedBookLoan.User.SSN);
//            Assert.Equal(expectedBookLoan.BookInstance.Id, retrievedBookLoan.BookInstance.Id);
//        }

//        [Fact]
//        public async Task ListUserBookLoans_WithValidSSN_ReturnsListOfBookLoans()
//        {
//            // Arrange
//            var userSSN = "123-45-6789";
//            var bookLoan1 = new BookLoan
//            {
//                LoanDate = DateTime.Now,
//                DueDate = DateTime.Now.AddDays(30),
//                ReturnDate = null,
//                User = new User { SSN = userSSN },
//                BookInstance = new BookInstance { Id = 123, IsLoaned = true, Book = new Book() }
//            };
//            var bookLoan2 = new BookLoan
//            {
//                LoanDate = DateTime.Now,
//                DueDate = DateTime.Now.AddDays(30),
//                ReturnDate = null,
//                User = new User { SSN = userSSN },
//                BookInstance = new BookInstance { Id = 124, IsLoaned = true, Book = new Book() }
//            };

//            // Act
//            await _loanRepository.CreateLoan(bookLoan1);
//            await _loanRepository.CreateLoan(bookLoan2);
//            var result = await _loanRepository.ListUserLoans(userSSN);
//            await _loanRepository.DeleteLoan(bookLoan1.Id);
//            await _loanRepository.DeleteLoan(bookLoan2.Id);

//            // Assert
//            Assert.Equal(2, result.Count);
//            Assert.Contains(result, r => r.Id == bookLoan1.Id && r is BookLoan);
//            Assert.Contains(result, r => r.Id == bookLoan2.Id && r is BookLoan);
//        }

//        [Fact]
//        public async Task UpdateBookLoan_UpdatesExistingBookLoan()
//        {
//            // Arrange
//            var bookLoanId = 1;
//            var originalBookLoan = new BookLoan
//            {
//                LoanDate = DateTime.Now,
//                DueDate = DateTime.Now.AddDays(30),
//                ReturnDate = null,
//                User = new User { SSN = "123-45-6789" },
//                BookInstance = new BookInstance { Id = 123, IsLoaned = true, Book = new Book() }
//            };
//            var updatedBookLoan = new BookLoan
//            {
//                LoanDate = DateTime.Now.AddDays(-1),
//                DueDate = DateTime.Now.AddDays(29),
//                ReturnDate = DateTime.Now,
//                User = new User { SSN = "123-45-6789" },
//                BookInstance = new BookInstance { Id = 123, IsLoaned = false, Book = new Book() }
//            };

//            // Act
//            await _loanRepository.CreateLoan(originalBookLoan);
//            var result = await _loanRepository.UpdateLoan(updatedBookLoan);
//            await _loanRepository.DeleteLoan(bookLoanId);

//            // Assert
//            var updatedLoan = Assert.IsType<BookLoan>(result);
//            Assert.NotNull(result);
//            Assert.Equal(updatedBookLoan.LoanDate, updatedLoan.LoanDate);
//            Assert.Equal(updatedBookLoan.DueDate, updatedLoan.DueDate);
//            Assert.Equal(updatedBookLoan.ReturnDate, updatedLoan.ReturnDate);
//            Assert.Equal(updatedBookLoan.User.SSN, updatedLoan.User.SSN);
//            Assert.Equal(updatedBookLoan.BookInstance.Id, updatedLoan.BookInstance.Id);
//            Assert.Equal(updatedBookLoan.BookInstance.IsLoaned, updatedLoan.BookInstance.IsLoaned);
//        }

//        [Fact]
//        public async Task DeleteBookLoan_RemovesBookLoanFromDatabase()
//        {
//            // Arrange
//            var bookLoanId = 1;
//            var bookLoan = new BookLoan
//            {
//                LoanDate = DateTime.Now,
//                DueDate = DateTime.Now.AddDays(30),
//                ReturnDate = null,
//                User = new User { SSN = "123-45-6789" },
//                BookInstance = new BookInstance { Id = 123, IsLoaned = true, Book = new Book() }
//            };

//            // Act
//            await _loanRepository.CreateLoan(bookLoan);
//            var deletedBookLoan = await _loanRepository.DeleteLoan(bookLoanId);
//            var result = await _loanRepository.GetLoan(bookLoanId);

//            // Assert
//            Assert.NotNull(deletedBookLoan);
//            Assert.Null(result);
//        }
//    }
//}
