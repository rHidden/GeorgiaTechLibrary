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
    public class LoanRepositoryTest
    {
        private Mock<IDatabaseConnectionFactory> _mockDatabaseConnectionFactory;
        private Mock<ILoanRepository> _mockLoanRepository;
        private string _connectionString;

        public LoanRepositoryTest()
        {
            _connectionString = DatabaseConnectionTest._connectionString;
            _mockDatabaseConnectionFactory = new Mock<IDatabaseConnectionFactory>();
            _mockDatabaseConnectionFactory.Setup(d => d.CreateConnection())
            .Returns(new SqlConnection(_connectionString));
            _mockLoanRepository = new Mock<ILoanRepository>();
        }

        [Fact]
        public async Task GetLoan_ValidId_ReturnsLoan()
        {
            // Arrange
            int loanId = 1;
            var expectedLoan = new Loan
            {
                Id = loanId,
                LoanDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(30),
                ReturnDate = null,
                User = new User { SSN = "123-45-6789" }
            };

            _mockLoanRepository.Setup(r => r.GetLoan(loanId)).ReturnsAsync(expectedLoan);

            // Act
            var result = await _mockLoanRepository.Object.GetLoan(loanId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedLoan.Id, result.Id);
            Assert.Equal(expectedLoan.LoanDate, result.LoanDate);
            Assert.Equal(expectedLoan.DueDate, result.DueDate);
            Assert.Equal(expectedLoan.ReturnDate, result.ReturnDate);
            Assert.Equal(expectedLoan.User.SSN, result.User.SSN);
        }

        [Fact]
        public async Task ListUserLoans_ValidSSN_ReturnsListofLoans()
        {
            // Arrange
            string userSSN = "123-45-6789";
            var loans = new List<Loan>
            {
                new Loan { Id = 1 },
                new Loan { Id = 2 }
            };

            _mockLoanRepository.Setup(r => r.ListUserLoans(userSSN)).ReturnsAsync(loans);

            // Act
            var result = await _mockLoanRepository.Object.ListUserLoans(userSSN);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(loans[0], result);
            Assert.Contains(loans[1], result);
        }

        [Fact]
        public async Task CreateLoan_ValidLoan_ReturnsCreatedDigitalItemLoan()
        {
            // Arrange
            var loan = new DigitalItemLoan
            {
                Id = 1,
                LoanDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(30),
                ReturnDate = null,
                User = new User { SSN = "123-45-6789" },
                DigitalItem = new DigitalItem { Id = 1, Format = "Test Format", Size = 100.00}
            };

            _mockLoanRepository.Setup(r => r.CreateLoan(loan)).ReturnsAsync(loan);

            // Act
            var result = await _mockLoanRepository.Object.CreateLoan(loan);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(loan.Id, result.Id);
            Assert.Equal(loan.LoanDate, result.LoanDate);
            Assert.Equal(loan.DueDate, result.DueDate);
            Assert.Equal(loan.ReturnDate, result.ReturnDate);
            Assert.Equal(loan.User.SSN, result.User.SSN);
        }

        [Fact]
        public async Task CreateLoan_ValidLoan_ReturnsCreatedLoan()
        {
            // Arrange
            var loan = new BookLoan
            {
                Id = 1,
                LoanDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(30),
                ReturnDate = null,
                User = new User { SSN = "123-45-6789" },
                BookInstance = new BookInstance { Id = 123, IsLoaned = true, Book = null }
            };

            _mockLoanRepository.Setup(r => r.CreateLoan(loan)).ReturnsAsync(loan);

            // Act
            var result = await _mockLoanRepository.Object.CreateLoan(loan);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(loan.Id, result.Id);
            Assert.Equal(loan.LoanDate, result.LoanDate);
            Assert.Equal(loan.DueDate, result.DueDate);
            Assert.Equal(loan.ReturnDate, result.ReturnDate);
            Assert.Equal(loan.User.SSN, result.User.SSN);
        }

        [Fact]
        public async Task UpdateLoan_ValidLoan_ReturnsUpdatedLoan()
        {
            // Arrange
            var loan = new Loan
            {
                Id = 1,
                LoanDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(30),
                ReturnDate = null,
                User = new User { SSN = "123-45-6789" }
            };

            _mockLoanRepository.Setup(r => r.UpdateLoan(loan)).ReturnsAsync(loan);

            // Act
            var result = await _mockLoanRepository.Object.UpdateLoan(loan);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(loan.Id, result.Id);
            Assert.Equal(loan.LoanDate, result.LoanDate);
            Assert.Equal(loan.DueDate, result.DueDate);
            Assert.Equal(loan.ReturnDate, result.ReturnDate);
            Assert.Equal(loan.User.SSN, result.User.SSN);
        }

        [Fact]
        public async Task DeleteLoan_ValidId_DeletesLoan()
        {
            // Arrange
            int loanId = 1;
            var loan = new Loan
            {
                Id = loanId,
                LoanDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(30),
                ReturnDate = null,
                User = new User { SSN = "123-45-6789" }
            };

            _mockLoanRepository.Setup(r => r.GetLoan(loanId)).ReturnsAsync(loan);
            _mockLoanRepository.Setup(r => r.DeleteLoan(loanId)).ReturnsAsync(true);

            // Act
            var deletedLoan = await _mockLoanRepository.Object.DeleteLoan(loanId);
            var result = await _mockLoanRepository.Object.GetLoan(loanId);

            // Assert
            Assert.NotNull(deletedLoan);
            Assert.Null(result);
        }
    }
}

