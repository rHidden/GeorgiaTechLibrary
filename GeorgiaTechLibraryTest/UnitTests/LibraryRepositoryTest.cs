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
    public class LibraryRepositoryTest
    {
        private Mock<IDatabaseConnectionFactory> _mockDatabaseConnectionFactory;
        private LibraryRepository _libraryRepository;
        private string _connectionString;

        public LibraryRepositoryTest()
        {
            _connectionString = DatabaseConnectionTest._connectionString;
            _mockDatabaseConnectionFactory = new Mock<IDatabaseConnectionFactory>();
            _mockDatabaseConnectionFactory.Setup(d => d.CreateConnection())
                .Returns(() => new SqlConnection(_connectionString));
            _libraryRepository = new LibraryRepository(_mockDatabaseConnectionFactory.Object);
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

            // Act
            var result = await _libraryRepository.CreateLibrary(newLibrary);
            await _libraryRepository.DeleteLibrary(newLibrary.Name);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(newLibrary.Name, result.Name);
            Assert.Equal(newLibrary.LibraryAddress.Street, result.LibraryAddress.Street);
            Assert.Equal(newLibrary.LibraryAddress.StreetNumber, result.LibraryAddress.StreetNumber);
            Assert.Equal(newLibrary.LibraryAddress.City, result.LibraryAddress.City);
            Assert.Equal(newLibrary.LibraryAddress.ZipCode, result.LibraryAddress.ZipCode);
        }

        //[Fact]
        //public async Task GetLibrary_WithValidName_ReturnsLibrary()
        //{
        //    // Arrange
        //    var libraryName = "Test Library";
        //    var expectedLibrary = new Library
        //    {
        //        Name = libraryName,
        //        LibraryAddress = new Address
        //        {
        //            Street = "123 Main St",
        //            StreetNumber = "456",
        //            City = "Anytown",
        //            ZipCode = "98765"
        //        }
        //    };

        //    // Act
        //    await _libraryRepository.CreateLibrary(expectedLibrary);
        //    var result = await _libraryRepository.GetLibrary(libraryName);
        //    await _libraryRepository.DeleteLibrary(libraryName);

        //    // Assert
        //    Assert.NotNull(result);
        //    Assert.Equal(expectedLibrary.Name, result.Name);
        //    Assert.Equal(expectedLibrary.LibraryAddress.Street, result.LibraryAddress.Street);
        //    Assert.Equal(expectedLibrary.LibraryAddress.StreetNumber, result.LibraryAddress.StreetNumber);
        //    Assert.Equal(expectedLibrary.LibraryAddress.City, result.LibraryAddress.City);
        //    Assert.Equal(expectedLibrary.LibraryAddress.ZipCode, result.LibraryAddress.ZipCode);
        //}

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

            // Act
            await _libraryRepository.CreateLibrary(library1);
            await _libraryRepository.CreateLibrary(library2);
            var result = await _libraryRepository.ListLibraries();
            await _libraryRepository.DeleteLibrary(library1.Name);
            await _libraryRepository.DeleteLibrary(library2.Name);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, r => r.Name == library1.Name && r.LibraryAddress.Street == library1.LibraryAddress.Street && r.LibraryAddress.StreetNumber == library1.LibraryAddress.StreetNumber && r.LibraryAddress.City == library1.LibraryAddress.City && r.LibraryAddress.ZipCode == library1.LibraryAddress.ZipCode);
            Assert.Contains(result, r => r.Name == library2.Name && r.LibraryAddress.Street == library2.LibraryAddress.Street && r.LibraryAddress.StreetNumber == library2.LibraryAddress.StreetNumber && r.LibraryAddress.City == library2.LibraryAddress.City && r.LibraryAddress.ZipCode == library2.LibraryAddress.ZipCode);
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

            // Act
            await _libraryRepository.CreateLibrary(originalLibrary);
            var result = await _libraryRepository.UpdateLibrary(updatedLibrary);
            await _libraryRepository.DeleteLibrary(libraryName);

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

            // Act
            await _libraryRepository.CreateLibrary(library);
            var deletedLibrary = await _libraryRepository.DeleteLibrary(libraryName);

            // Assert
            Assert.NotNull(deletedLibrary);
        }
    }
}
