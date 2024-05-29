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
using Microsoft.SqlServer.Server;

namespace GeorgiaTechLibraryTest.UnitTests
{
    public class DigitalItemLoanRepositoryTest
    {
        private Mock<IDatabaseConnectionFactory> _mockDatabaseConnectionFactory;
        private LoanRepository _loanRepository;
        private DigitalItemRepository _digitalItemRepository;
        private MemberRepository _memberRepository;
        private string _connectionString;

        public DigitalItemLoanRepositoryTest()
        {
            _connectionString = DatabaseConnectionTest._connectionString;
            _mockDatabaseConnectionFactory = new Mock<IDatabaseConnectionFactory>();
            _mockDatabaseConnectionFactory.Setup(d => d.CreateConnection())
                .Returns(() => new SqlConnection(_connectionString));
            _loanRepository = new LoanRepository(_mockDatabaseConnectionFactory.Object);
            _digitalItemRepository = new DigitalItemRepository(_mockDatabaseConnectionFactory.Object);
            _memberRepository = new MemberRepository(_mockDatabaseConnectionFactory.Object);
        }

        [Fact]
        public async Task CreateDigitalItemLoan_CreatesNewDigitalItemLoan()
        {
            // Arrange
            var user = new Member { UserAddress = new Address { Street = "Main St", StreetNumber = "1", City = "City", ZipCode = "12345" }, SSN = "13" };

            var text = new Text
            {
                Id = 1,
                Name = "Test Text",
                Format = "pdf",
                Size = 1.0,
                Authors = new List<string> { "Author1" }
            };

            var newDigitalItemLoan = new DigitalItemLoan
            {
                Id = 6,
                LoanDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(30),
                ReturnDate = null,
                User = user,
                DigitalItem = text
            };

            // Act
            await _memberRepository.CreateMember(user);
            await _digitalItemRepository.CreateText(text);
            var result = await _loanRepository.CreateLoan(newDigitalItemLoan);
            await _loanRepository.DeleteLoan(result.Id);
            await _digitalItemRepository.DeleteDigitalItem(text.Id);
            await _memberRepository.DeleteMember(user.SSN);

            // Assert
            var createdDigitalItemLoan = Assert.IsType<DigitalItemLoan>(result);
            Assert.NotNull(result);
            Assert.Equal(newDigitalItemLoan.LoanDate, createdDigitalItemLoan.LoanDate);
            Assert.Equal(newDigitalItemLoan.DueDate, createdDigitalItemLoan.DueDate);
            Assert.Null(createdDigitalItemLoan.ReturnDate);
            Assert.Equal(newDigitalItemLoan.User.SSN, createdDigitalItemLoan.User.SSN);
            Assert.Equal(newDigitalItemLoan.DigitalItem.Id, createdDigitalItemLoan.DigitalItem.Id);
        }

        [Fact]
        public async Task GetDigitalItemLoan_WithValidId_ReturnsDigitalItemLoan()
        {
            // Arrange
            var user = new Member { UserAddress = new Address { Street = "Main St", StreetNumber = "1", City = "City", ZipCode = "12345" }, SSN = "14" };

            var text = new Text
            {
                Id = 2,
                Name = "Test Text",
                Format = "pdf",
                Size = 1.0,
                Authors = new List<string> { "Author1" }
            };

            var expectedDigitalItemLoan = new DigitalItemLoan
            {

                Id = 7,
                LoanDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(30),
                ReturnDate = null,
                User = user,
                DigitalItem = text
            };

            // Act
            await _memberRepository.CreateMember(user);
            await _digitalItemRepository.CreateText(text);
            await _loanRepository.CreateLoan(expectedDigitalItemLoan);
            var result = await _loanRepository.GetLoan(expectedDigitalItemLoan.Id);
            await _loanRepository.DeleteLoan(expectedDigitalItemLoan.Id);
            await _digitalItemRepository.DeleteDigitalItem(text.Id);
            await _memberRepository.DeleteMember(user.SSN);

            // Assert
            var retrievedDigitalItemLoan = Assert.IsType<DigitalItemLoan>(result);
            Assert.NotNull(result);
            //Assert.Equal(expectedDigitalItemLoan.LoanDate, retrievedDigitalItemLoan.LoanDate);
            //Assert.Equal(expectedDigitalItemLoan.DueDate, retrievedDigitalItemLoan.DueDate);
            //Assert.Null(retrievedDigitalItemLoan.ReturnDate);
            //Assert.Equal(expectedDigitalItemLoan.User.SSN, retrievedDigitalItemLoan.User.SSN);
            //Assert.Equal(expectedDigitalItemLoan.DigitalItem.Id, retrievedDigitalItemLoan.DigitalItem.Id);
        }

        [Fact]
        public async Task ListUserDigitalItemLoans_WithValidSSN_ReturnsListOfDigitalItemLoans()
        {
            // Arrange
            var user = new Member { UserAddress = new Address { Street = "Main St", StreetNumber = "1", City = "City", ZipCode = "12345" }, SSN = "15" };

            var text1 = new Text
            {
                Id = 3,
                Name = "Test Text",
                Format = "pdf",
                Size = 1.0,
                Authors = new List<string> { "Author1" }
            };

            var text2 = new Text
            {
                Id = 4,
                Name = "Test Text",
                Format = "pdf",
                Size = 1.0,
                Authors = new List<string> { "Author1" }
            };

            var digitalItemLoan1 = new DigitalItemLoan
            {
                Id = 8,
                LoanDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(30),
                ReturnDate = DateTime.Now.AddDays(1),
                User = user,
                DigitalItem = text1,
                
            };

            var digitalItemLoan2 = new DigitalItemLoan
            {
                Id = 9,
                LoanDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(30),
                ReturnDate = DateTime.Now.AddDays(1),
                User = user,
                DigitalItem = text2
            };

            // Act
            await _memberRepository.CreateMember(user);
            await _digitalItemRepository.CreateText(text1);
            await _digitalItemRepository.CreateText(text2);
            await _loanRepository.CreateLoan(digitalItemLoan1);
            await _loanRepository.CreateLoan(digitalItemLoan2);
            var result = await _loanRepository.ListUserLoans(user.SSN);
            await _loanRepository.DeleteLoan(digitalItemLoan1.Id);
            await _loanRepository.DeleteLoan(digitalItemLoan2.Id);
            await _digitalItemRepository.DeleteDigitalItem(text1.Id);
            await _digitalItemRepository.DeleteDigitalItem(text2.Id);
            await _memberRepository.DeleteMember(user.SSN);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, r => r.Id == digitalItemLoan1.Id && r is DigitalItemLoan);
            Assert.Contains(result, r => r.Id == digitalItemLoan2.Id && r is DigitalItemLoan);
        }

        [Fact]
        public async Task UpdateDigitalItemLoan_UpdatesExistingDigitalItemLoan()
        {
            // Arrange
            var user = new Member { UserAddress = new Address { Street = "Main St", StreetNumber = "1", City = "City", ZipCode = "12345" }, SSN = "16" };

            var text = new Text
            {
                Id = 5,
                Name = "Test Text",
                Format = "pdf",
                Size = 1.0,
                Authors = new List<string> { "Author1" }
            };

            var originalDigitalItemLoan = new DigitalItemLoan
            {
                Id = 10,
                LoanDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(30),
                ReturnDate = null,
                User = user,
                DigitalItem = text
            };

            var updatedDigitalItemLoan = new DigitalItemLoan
            {
                Id = 10,
                LoanDate = DateTime.Now.AddDays(-1),
                DueDate = DateTime.Now.AddDays(29),
                ReturnDate = DateTime.Now,
                User = user,
                DigitalItem = text
            };

            // Act
            await _memberRepository.CreateMember(user);
            await _digitalItemRepository.CreateText(text);
            await _loanRepository.CreateLoan(originalDigitalItemLoan);
            var result = await _loanRepository.UpdateLoan(updatedDigitalItemLoan);
            await _loanRepository.DeleteLoan(updatedDigitalItemLoan.Id);
            await _digitalItemRepository.DeleteDigitalItem(text.Id);
            await _memberRepository.DeleteMember(user.SSN);

            // Assert
            var updatedLoan = Assert.IsType<DigitalItemLoan>(result);
            Assert.NotNull(result);
            Assert.Equal(updatedDigitalItemLoan.LoanDate, updatedLoan.LoanDate);
            Assert.Equal(updatedDigitalItemLoan.DueDate, updatedLoan.DueDate);
            Assert.Equal(updatedDigitalItemLoan.ReturnDate, updatedLoan.ReturnDate);
            Assert.Equal(updatedDigitalItemLoan.User.SSN, updatedLoan.User.SSN);
            Assert.Equal(updatedDigitalItemLoan.DigitalItem.Id, updatedLoan.DigitalItem.Id);
        }

        [Fact]
        public async Task DeleteDigitalItemLoan_RemovesDigitalItemLoanFromDatabase()
        {
            // Arrange
            var user = new Member { UserAddress = new Address { Street = "Main St", StreetNumber = "1", City = "City", ZipCode = "12345" }, SSN = "17" };

            var text = new Text
            {
                Id = 6,
                Name = "Test Text",
                Format = "pdf",
                Size = 1.0,
                Authors = new List<string> { "Author1" }
            };

            var digitalItemLoan = new DigitalItemLoan
            {

                Id = 11,
                LoanDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(30),
                ReturnDate = null,
                User = user,
                DigitalItem = text
            };

            // Act
            await _memberRepository.CreateMember(user);
            await _digitalItemRepository.CreateText(text);
            await _loanRepository.CreateLoan(digitalItemLoan);
            var deletedDigitalItemLoan = await _loanRepository.DeleteLoan(digitalItemLoan.Id);
            var result = await _loanRepository.GetLoan(digitalItemLoan.Id);
            await _digitalItemRepository.DeleteDigitalItem(text.Id);
            await _memberRepository.DeleteMember(user.SSN);

            // Assert
            Assert.NotNull(deletedDigitalItemLoan);
            Assert.Null(result);
        }
    }
}

