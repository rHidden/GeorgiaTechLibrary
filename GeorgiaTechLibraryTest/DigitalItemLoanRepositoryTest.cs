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
//using Microsoft.SqlServer.Server;

//namespace GeorgiaTechLibraryTest
//{
//    public class DigitalItemLoanRepositoryTest
//    {
//        private Mock<IDatabaseConnectionFactory> _mockDatabaseConnectionFactory;
//        private LoanRepository _loanRepository;
//        private string _connectionString;

//        public DigitalItemLoanRepositoryTest()
//        {
//            _connectionString = DatabaseConnectionTest._connectionString;
//            _mockDatabaseConnectionFactory = new Mock<IDatabaseConnectionFactory>();
//            _mockDatabaseConnectionFactory.Setup(d => d.CreateConnection())
//                .Returns(() => new SqlConnection(_connectionString));
//            _loanRepository = new LoanRepository(_mockDatabaseConnectionFactory.Object);
//        }

//        [Fact]
//        public async Task CreateDigitalItemLoan_CreatesNewDigitalItemLoan()
//        {
//            // Arrange
//            var newDigitalItemLoan = new DigitalItemLoan
//            {
//                LoanDate = DateTime.Now,
//                DueDate = DateTime.Now.AddDays(30),
//                ReturnDate = null,
//                User = new User { SSN = "123-45-6789" },
//                DigitalItem = new DigitalItem { Id = 1, Format = "PDF", Size = 100.00 }
//            };

//            // Act
//            var result = await _loanRepository.CreateLoan(newDigitalItemLoan);
//            await _loanRepository.DeleteLoan(result.Id);

//            // Assert
//            var createdDigitalItemLoan = Assert.IsType<DigitalItemLoan>(result);
//            Assert.NotNull(result);
//            Assert.Equal(newDigitalItemLoan.LoanDate, createdDigitalItemLoan.LoanDate);
//            Assert.Equal(newDigitalItemLoan.DueDate, createdDigitalItemLoan.DueDate);
//            Assert.Null(createdDigitalItemLoan.ReturnDate);
//            Assert.Equal(newDigitalItemLoan.User.SSN, createdDigitalItemLoan.User.SSN);
//            Assert.Equal(newDigitalItemLoan.DigitalItem.Id, createdDigitalItemLoan.DigitalItem.Id);
//        }

//        [Fact]
//        public async Task GetDigitalItemLoan_WithValidId_ReturnsDigitalItemLoan()
//        {
//            // Arrange
//            var digitalItemLoanId = 1;
//            var expectedDigitalItemLoan = new DigitalItemLoan
//            {
//                LoanDate = DateTime.Now,
//                DueDate = DateTime.Now.AddDays(30),
//                ReturnDate = null,
//                User = new User { SSN = "123-45-6789" },
//                DigitalItem = new DigitalItem { Id = 1, Format = "PDF", Size = 100.00 }
//            };

//            // Act
//            await _loanRepository.CreateLoan(expectedDigitalItemLoan);
//            var result = await _loanRepository.GetLoan(digitalItemLoanId);
//            await _loanRepository.DeleteLoan(digitalItemLoanId);

//            // Assert
//            var retrievedDigitalItemLoan = Assert.IsType<DigitalItemLoan>(result);
//            Assert.NotNull(result);
//            Assert.Equal(expectedDigitalItemLoan.LoanDate, retrievedDigitalItemLoan.LoanDate);
//            Assert.Equal(expectedDigitalItemLoan.DueDate, retrievedDigitalItemLoan.DueDate);
//            Assert.Null(retrievedDigitalItemLoan.ReturnDate);
//            Assert.Equal(expectedDigitalItemLoan.User.SSN, retrievedDigitalItemLoan.User.SSN);
//            Assert.Equal(expectedDigitalItemLoan.DigitalItem.Id, retrievedDigitalItemLoan.DigitalItem.Id);
//        }

//        [Fact]
//        public async Task ListUserDigitalItemLoans_WithValidSSN_ReturnsListOfDigitalItemLoans()
//        {
//            // Arrange
//            var userSSN = "123-45-6789";
//            var digitalItemLoan1 = new DigitalItemLoan
//            {
//                LoanDate = DateTime.Now,
//                DueDate = DateTime.Now.AddDays(30),
//                ReturnDate = null,
//                User = new User { SSN = userSSN },
//                DigitalItem = new DigitalItem { Id = 1, Format = "PDF", Size = 100.00 }
//            };
//            var digitalItemLoan2 = new DigitalItemLoan
//            {
//                LoanDate = DateTime.Now,
//                DueDate = DateTime.Now.AddDays(30),
//                ReturnDate = null,
//                User = new User { SSN = userSSN },
//                DigitalItem = new DigitalItem { Id = 2, Format = "EPUB", Size = 200.00 }
//            };

//            // Act
//            await _loanRepository.CreateLoan(digitalItemLoan1);
//            await _loanRepository.CreateLoan(digitalItemLoan2);
//            var result = await _loanRepository.ListUserLoans(userSSN);
//            await _loanRepository.DeleteLoan(digitalItemLoan1.Id);
//            await _loanRepository.DeleteLoan(digitalItemLoan2.Id);

//            // Assert
//            Assert.Equal(2, result.Count);
//            Assert.Contains(result, r => r.Id == digitalItemLoan1.Id && r is DigitalItemLoan);
//            Assert.Contains(result, r => r.Id == digitalItemLoan2.Id && r is DigitalItemLoan);
//        }

//        [Fact]
//        public async Task UpdateDigitalItemLoan_UpdatesExistingDigitalItemLoan()
//        {
//            // Arrange
//            var digitalItemLoanId = 1;
//            var originalDigitalItemLoan = new DigitalItemLoan
//            {
//                LoanDate = DateTime.Now,
//                DueDate = DateTime.Now.AddDays(30),
//                ReturnDate = null,
//                User = new User { SSN = "123-45-6789" },
//                DigitalItem = new DigitalItem { Id = 1, Format = "PDF", Size = 100.00 }
//            };
//            var updatedDigitalItemLoan = new DigitalItemLoan
//            {
//                LoanDate = DateTime.Now.AddDays(-1),
//                DueDate = DateTime.Now.AddDays(29),
//                ReturnDate = DateTime.Now,
//                User = new User { SSN = "123-45-6789" },
//                DigitalItem = new DigitalItem { Id = 1, Format = "EPUB", Size = 200.00 }
//            };

//            // Act
//            await _loanRepository.CreateLoan(originalDigitalItemLoan);
//            var result = await _loanRepository.UpdateLoan(updatedDigitalItemLoan);
//            await _loanRepository.DeleteLoan(digitalItemLoanId);

//            // Assert
//            var updatedLoan = Assert.IsType<DigitalItemLoan>(result);
//            Assert.NotNull(result);
//            Assert.Equal(updatedDigitalItemLoan.LoanDate, updatedLoan.LoanDate);
//            Assert.Equal(updatedDigitalItemLoan.DueDate, updatedLoan.DueDate);
//            Assert.Equal(updatedDigitalItemLoan.ReturnDate, updatedLoan.ReturnDate);
//            Assert.Equal(updatedDigitalItemLoan.User.SSN, updatedLoan.User.SSN);
//            Assert.Equal(updatedDigitalItemLoan.DigitalItem.Id, updatedLoan.DigitalItem.Id);
//        }

//        [Fact]
//        public async Task DeleteDigitalItemLoan_RemovesDigitalItemLoanFromDatabase()
//        {
//            // Arrange
//            var digitalItemLoanId = 1;
//            var digitalItemLoan = new DigitalItemLoan
//            {
//                LoanDate = DateTime.Now,
//                DueDate = DateTime.Now.AddDays(30),
//                ReturnDate = null,
//                User = new User { SSN = "123-45-6789" },
//                DigitalItem = new DigitalItem { Id = 1, Format = "PDF", Size = 100.00 }
//            };

//            // Act
//            await _loanRepository.CreateLoan(digitalItemLoan);
//            var deletedDigitalItemLoan = await _loanRepository.DeleteLoan(digitalItemLoanId);
//            var result = await _loanRepository.GetLoan(digitalItemLoanId);

//            // Assert
//            Assert.NotNull(deletedDigitalItemLoan);
//            Assert.Null(result);
//        }
//    }
//}

