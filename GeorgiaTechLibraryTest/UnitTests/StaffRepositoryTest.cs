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

namespace GeorgiaTechLibraryTest.UnitTests
{
    public class StaffRepositoryTest
    {
        private Mock<IDatabaseConnectionFactory> _mockDatabaseConnectionFactory;
        private StaffRepository _staffRepository;
        private string _connectionString;

        public StaffRepositoryTest()
        {
            _connectionString = DatabaseConnectionTest._connectionString;
            _mockDatabaseConnectionFactory = new Mock<IDatabaseConnectionFactory>();
            _mockDatabaseConnectionFactory.Setup(d => d.CreateConnection())
                .Returns(() => new SqlConnection(_connectionString));
            _staffRepository = new StaffRepository(_mockDatabaseConnectionFactory.Object);
        }

        [Fact]
        public async Task CreateStaff_CreatesNewStaff()
        {
            // Arrange
            var newStaff = new Staff
            {
                SSN = "7",
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "1234567890",
                UserAddress = new Address { Street = "Main St", StreetNumber = "1", City = "City", ZipCode = "12345" },
                LibrarianNumber = "123456",
                Role = "Librarian"
            };

            // Act
            var result = await _staffRepository.CreateStaff(newStaff);
            await _staffRepository.DeleteStaff(newStaff.SSN);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(newStaff.SSN, result.SSN);
            Assert.Equal(newStaff.FirstName, result.FirstName);
            Assert.Equal(newStaff.LastName, result.LastName);
            Assert.Equal(newStaff.PhoneNumber, result.PhoneNumber);
            Assert.Equal(newStaff.UserAddress.Street, result.UserAddress.Street);
            Assert.Equal(newStaff.LibrarianNumber, result.LibrarianNumber);
            Assert.Equal(newStaff.Role, result.Role);
        }

        [Fact]
        public async Task GetStaff_WithValidSSN_ReturnsStaff()
        {
            // Arrange
            var expectedStaff = new Staff
            {
                SSN = "8",
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "1234567890",
                UserAddress = new Address { Street = "Main St", StreetNumber = "1", City = "City", ZipCode = "12345" },
                LibrarianNumber = "123456",
                Role = "Librarian"
            };

            // Act
            await _staffRepository.CreateStaff(expectedStaff);
            var result = await _staffRepository.GetStaff(expectedStaff.SSN);
            await _staffRepository.DeleteStaff(expectedStaff.SSN);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedStaff.SSN, result.SSN);
            Assert.Equal(expectedStaff.FirstName, result.FirstName);
            Assert.Equal(expectedStaff.LastName, result.LastName);
            Assert.Equal(expectedStaff.PhoneNumber, result.PhoneNumber);
            Assert.Equal(expectedStaff.UserAddress.Street, result.UserAddress.Street);
            Assert.Equal(expectedStaff.LibrarianNumber, result.LibrarianNumber);
            Assert.Equal(expectedStaff.Role, result.Role);
        }

        [Fact]
        public async Task ListStaff_ReturnsListOfStaff()
        {
            // Arrange
            var staff1 = new Staff
            {
                SSN = "9",
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "1234567890",
                UserAddress = new Address { Street = "Main St", StreetNumber = "1", City = "City", ZipCode = "12345" },
                LibrarianNumber = "123456",
                Role = "Librarian"
            };
            var staff2 = new Staff
            {
                SSN = "10",
                FirstName = "Jane",
                LastName = "Doe",
                PhoneNumber = "0987654321",
                UserAddress = new Address { Street = "Second St", StreetNumber = "2", City = "City", ZipCode = "54321" },
                LibrarianNumber = "654321",
                Role = "Assistant Librarian"
            };

            // Act
            await _staffRepository.CreateStaff(staff1);
            await _staffRepository.CreateStaff(staff2);
            var result = await _staffRepository.ListStaff();
            await _staffRepository.DeleteStaff(staff1.SSN);
            await _staffRepository.DeleteStaff(staff2.SSN);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, r => r.SSN == staff1.SSN && r.FirstName == staff1.FirstName && r.LastName == staff1.LastName && r.Role == staff1.Role);
            Assert.Contains(result, r => r.SSN == staff2.SSN && r.FirstName == staff2.FirstName && r.LastName == staff2.LastName && r.Role == staff2.Role);
        }

        [Fact]
        public async Task UpdateStaff_UpdatesExistingStaff()
        {
            // Arrange
            var originalStaff = new Staff
            {
                SSN = "11",
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "1234567890",
                UserAddress = new Address { Street = "Main St", StreetNumber = "1", City = "City", ZipCode = "12345" },
                LibrarianNumber = "123456",
                Role = "Librarian"
            };
            var updatedStaff = new Staff
            {
                SSN = "11",
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "0987654321",
                UserAddress = new Address { Street = "Main St", StreetNumber = "1", City = "City", ZipCode = "12345" },
                LibrarianNumber = "654321",
                Role = "Senior Librarian"
            };

            // Act
            await _staffRepository.CreateStaff(originalStaff);
            var result = await _staffRepository.UpdateStaff(updatedStaff);
            await _staffRepository.DeleteStaff(updatedStaff.SSN);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(updatedStaff.SSN, result.SSN);
            Assert.Equal(updatedStaff.FirstName, result.FirstName);
            Assert.Equal(updatedStaff.LastName, result.LastName);
            Assert.Equal(updatedStaff.PhoneNumber, result.PhoneNumber);
            Assert.Equal(updatedStaff.UserAddress.Street, result.UserAddress.Street);
            Assert.Equal(updatedStaff.LibrarianNumber, result.LibrarianNumber);
            Assert.Equal(updatedStaff.Role, result.Role);
        }

        [Fact]
        public async Task DeleteStaff_RemovesStaffFromDatabase()
        {
            // Arrange
            var staff = new Staff
            {
                SSN = "12",
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "1234567890",
                UserAddress = new Address { Street = "Main St", StreetNumber = "1", City = "City", ZipCode = "12345" },
                LibrarianNumber = "123456",
                Role = "Librarian"
            };

            // Act
            await _staffRepository.CreateStaff(staff);
            var result = await _staffRepository.DeleteStaff(staff.SSN);

            // Assert
            Assert.True(result);
        }
    }
}
