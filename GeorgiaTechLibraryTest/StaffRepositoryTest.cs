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
            var ssn = "123456789";
            var newStaff = new Staff
            {
                SSN = ssn,
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "1234567890",
                UserAddress = new Address { Street = "Main St", StreetNumber = "1", City = "City", ZipCode = "12345" },
                LibrarianNumber = "123456",
                Role = "Librarian"
            };

            // Act
            var result = await _staffRepository.CreateStaff(newStaff);
            await _staffRepository.DeleteStaff(ssn);

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
            var ssn = "123456789";
            var expectedStaff = new Staff
            {
                SSN = ssn,
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "1234567890",
                UserAddress = new Address { Street = "Main St", StreetNumber = "1", City = "City", ZipCode = "12345" },
                LibrarianNumber = "123456",
                Role = "Librarian"
            };

            // Act
            await _staffRepository.CreateStaff(expectedStaff);
            var result = await _staffRepository.GetStaff(ssn);
            await _staffRepository.DeleteStaff(ssn);

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
            var ssn1 = "123456789";
            var ssn2 = "987654321";
            var staff1 = new Staff
            {
                SSN = ssn1,
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "1234567890",
                UserAddress = new Address { Street = "Main St", StreetNumber = "1", City = "City", ZipCode = "12345" },
                LibrarianNumber = "123456",
                Role = "Librarian"
            };
            var staff2 = new Staff
            {
                SSN = ssn2,
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
            await _staffRepository.DeleteStaff(ssn1);
            await _staffRepository.DeleteStaff(ssn2);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, r => r.SSN == staff1.SSN && r.FirstName == staff1.FirstName && r.LastName == staff1.LastName && r.Role == staff1.Role);
            Assert.Contains(result, r => r.SSN == staff2.SSN && r.FirstName == staff2.FirstName && r.LastName == staff2.LastName && r.Role == staff2.Role);
        }

        [Fact]
        public async Task UpdateStaff_UpdatesExistingStaff()
        {
            // Arrange
            var ssn = "123456789";
            var originalStaff = new Staff
            {
                SSN = ssn,
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "1234567890",
                UserAddress = new Address { Street = "Main St", StreetNumber = "1", City = "City", ZipCode = "12345" },
                LibrarianNumber = "123456",
                Role = "Librarian"
            };
            var updatedStaff = new Staff
            {
                SSN = ssn,
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
            await _staffRepository.DeleteStaff(ssn);

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
            var ssn = "123456789";
            var staff = new Staff
            {
                SSN = ssn,
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "1234567890",
                UserAddress = new Address { Street = "Main St", StreetNumber = "1", City = "City", ZipCode = "12345" },
                LibrarianNumber = "123456",
                Role = "Librarian"
            };

            // Act
            await _staffRepository.CreateStaff(staff);
            var result = await _staffRepository.DeleteStaff(ssn);

            // Assert
            Assert.True(result);
        }
    }
}
