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
    public class LibraryRepositoryTest
    {
        private Mock<IDatabaseConnectionFactory> _mockDatabaseConnectionFactory;
        private Mock<ILibraryRepository> _mockLibraryRepository;
        private string _connectionString;

        public LibraryRepositoryTest()
        {
            _connectionString = DatabaseConnectionTest._connectionString;
            _mockDatabaseConnectionFactory = new Mock<IDatabaseConnectionFactory>();
            _mockDatabaseConnectionFactory.Setup(d => d.CreateConnection())
             .Returns(new SqlConnection(_connectionString));
            _mockLibraryRepository = new Mock<ILibraryRepository>();
        }

        [Fact]
        public async Task CreateLibrary_CreatesNewLibrary()
        {
            // Arrange
            var newLibrary = new Library
            {
                Name = "Test Library",
                LibraryAddress = new Address
                {
                    Street = "123 Main St",
                    StreetNumber = "456",
                    City = "Anytown",
                    ZipCode = "98765"
                }
            };

            _mockLibraryRepository.Setup(r => r.CreateLibrary(newLibrary)).ReturnsAsync(newLibrary);

            // Act
            var result = await _mockLibraryRepository.Object.CreateLibrary(newLibrary);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(newLibrary.Name, result.Name);
            Assert.Equal(newLibrary.LibraryAddress.Street, result.LibraryAddress.Street);
            Assert.Equal(newLibrary.LibraryAddress.StreetNumber, result.LibraryAddress.StreetNumber);
            Assert.Equal(newLibrary.LibraryAddress.City, result.LibraryAddress.City);
            Assert.Equal(newLibrary.LibraryAddress.ZipCode, result.LibraryAddress.ZipCode);
        }

        [Fact]
        public async Task GetLibrary_WithValidName_ReturnsLibrary()
        {
            // Arrange
            var libraryName = "Test Library";
            var expectedLibrary = new Library
            {
                Name = libraryName,
                LibraryAddress = new Address
                {
                    Street = "123 Main St",
                    StreetNumber = "456",
                    City = "Anytown",
                    ZipCode = "98765"
                }
            };

            _mockLibraryRepository.Setup(r => r.GetLibrary(libraryName)).ReturnsAsync(expectedLibrary);

            // Act
            var result = await _mockLibraryRepository.Object.GetLibrary(libraryName);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedLibrary.Name, result.Name);
            Assert.Equal(expectedLibrary.LibraryAddress.Street, result.LibraryAddress.Street);
            Assert.Equal(expectedLibrary.LibraryAddress.StreetNumber, result.LibraryAddress.StreetNumber);
            Assert.Equal(expectedLibrary.LibraryAddress.City, result.LibraryAddress.City);
            Assert.Equal(expectedLibrary.LibraryAddress.ZipCode, result.LibraryAddress.ZipCode);
        }

        [Fact]
        public async Task ListLibraries_ReturnsListOfLibraries()
        {
            // Arrange
            var library1 = new Library
            {
                Name = "Test Library 1",
                LibraryAddress = new Address
                {
                    Street = "123 Main St",
                    StreetNumber = "456",
                    City = "Anytown",
                    ZipCode = "98765"
                }
            };
            var library2 = new Library
            {
                Name = "Test Library 2",
                LibraryAddress = new Address
                {
                    Street = "789 Elm St",
                    StreetNumber = "321",
                    City = "Othertown",
                    ZipCode = "54321"
                }
            };
            var libraries = new List<Library> { library1, library2 };

            _mockLibraryRepository.Setup(r => r.ListLibraries()).ReturnsAsync(libraries);

            // Act
            var result = await _mockLibraryRepository.Object.ListLibraries();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(library1, result);
            Assert.Contains(library2, result);
        }

        [Fact]
        public async Task UpdateLibrary_UpdatesExistingLibrary()
        {
            // Arrange
            var libraryName = "Test Library";
            var originalLibrary = new Library
            {
                Name = libraryName,
                LibraryAddress = new Address
                {
                    Street = "123 Main St",
                    StreetNumber = "456",
                    City = "Anytown",
                    ZipCode = "98765"
                }
            };
            var updatedLibrary = new Library
            {
                Name = libraryName,
                LibraryAddress = new Address
                {
                    Street = "789 Elm St",
                    StreetNumber = "321",
                    City = "Othertown",
                    ZipCode = "54321"
                }
            };

            _mockLibraryRepository.Setup(r => r.UpdateLibrary(updatedLibrary)).ReturnsAsync(updatedLibrary);

            // Act
            var result = await _mockLibraryRepository.Object.UpdateLibrary(updatedLibrary);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(updatedLibrary.Name, result.Name);
            Assert.Equal(updatedLibrary.LibraryAddress.Street, result.LibraryAddress.Street);
            Assert.Equal(updatedLibrary.LibraryAddress.StreetNumber, result.LibraryAddress.StreetNumber);
            Assert.Equal(updatedLibrary.LibraryAddress.City, result.LibraryAddress.City);
            Assert.Equal(updatedLibrary.LibraryAddress.ZipCode, result.LibraryAddress.ZipCode);
        }

        [Fact]
        public async Task DeleteLibrary_RemovesLibraryFromDatabase()
        {
            // Arrange
            var libraryName = "Test Library";
            var library = new Library
            {
                Name = libraryName,
                LibraryAddress = new Address
                {
                    Street = "123 Main St",
                    StreetNumber = "456",
                    City = "Anytown",
                    ZipCode = "98765"
                }
            };

            _mockLibraryRepository.Setup(r => r.DeleteLibrary(libraryName)).ReturnsAsync(true);
            _mockLibraryRepository.Setup(r => r.GetLibrary(libraryName)).ReturnsAsync((Library)null);

            // Act
            var deletedLibrary = await _mockLibraryRepository.Object.DeleteLibrary(libraryName);
            var result = await _mockLibraryRepository.Object.GetLibrary(libraryName);

            // Assert
            Assert.NotNull(deletedLibrary);
            Assert.Null(result);
        }
    }
}
