using DataAccess.Models;
using GeorgiaTechLibrary.Controllers;
using GeorgiaTechLibrary.Services.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace GeorgiaTechLibraryTests.IntegrationTests
{
    public class LibraryControllerTest
    {
        private readonly Mock<ILibraryService> _libraryServiceMock;
        private readonly LibraryController _libraryController;
        private Library _library;
        private List<Library> _libraries;
        private string _name;

        public LibraryControllerTest()
        {
            _libraryServiceMock = new Mock<ILibraryService>();
            _libraryController = new LibraryController(_libraryServiceMock.Object);
        }

        private void SetUp()
        {
            _name = "Main Library";

            _libraries = new List<Library>
            {
                new Library
                {
                    Name = "Test Library 1",
                    LibraryAddress = new Address
                    {
                        Street = "123 Main St",
                        StreetNumber = "456",
                        City = "Anytown",
                        ZipCode = "98765"
                    }
                },
                new Library
                {
                    Name = "Test Library 2",
                    LibraryAddress = new Address
                    {
                        Street = "123 Secondary St",
                        StreetNumber = "456",
                        City = "Anytown",
                        ZipCode = "98765"
                    }
                }
            };

            _library = new Library
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
        }

        [Fact]
        public async Task GetLibrary_ReturnsOkResult_WithLibrary()
        {
            // Arrange
            SetUp();
            _libraryServiceMock.Setup(x => x.GetLibrary(_name)).ReturnsAsync(_library);

            // Act
            IActionResult result = await _libraryController.GetLibrary(_name);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(_library, okResult.Value);
        }

        //[Fact]
        //public async Task GetLibrary_ReturnsNotFound_WhenLibraryNotExists()
        //{
        //    // Arrange
        //    SetUp();
        //    _libraryServiceMock.Setup(x => x.GetLibrary(_name)).ReturnsAsync((Library)null);

        //    // Act
        //    IActionResult result = await _libraryController.GetLibrary(_name);

        //    // Assert
        //    Assert.IsType<NotFoundResult>(result);
        //}

        [Fact]
        public async Task ListLibraries_ReturnsOkResult_WithLibraries()
        {
            // Arrange
            SetUp();
            _libraryServiceMock.Setup(x => x.ListLibraries()).ReturnsAsync(_libraries);

            // Act
            IActionResult result = await _libraryController.ListLibraries();

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(_libraries, okResult.Value);
        }

        [Fact]
        public async Task CreateLibrary_ReturnsOkResult_WithCreatedLibrary()
        {
            // Arrange
            SetUp();
            _libraryServiceMock.Setup(x => x.CreateLibrary(_library)).ReturnsAsync(_library);

            // Act
            IActionResult result = await _libraryController.CreateLibrary(_library);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(_library, okResult.Value);
        }

        [Fact]
        public async Task UpdateLibrary_ReturnsOkResult_WithUpdatedLibrary()
        {
            // Arrange
            SetUp();
            _library.Name = "updated library";
            _libraryServiceMock.Setup(x => x.UpdateLibrary(_library)).ReturnsAsync(_library);

            // Act
            IActionResult result = await _libraryController.UpdateLibrary(_library);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(_library, okResult.Value);
        }

        [Fact]
        public async Task DeleteLibrary_ReturnsOkResult_WithDeleteSuccess()
        {
            // Arrange
            SetUp();
            _libraryServiceMock.Setup(x => x.DeleteLibrary(_name)).ReturnsAsync(true);

            // Act
            IActionResult result = await _libraryController.DeleteLibrary(_name);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(true, okResult.Value);
        }
    }
}
