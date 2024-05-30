using DataAccess.Models;
using GeorgiaTechLibrary.Controllers;
using GeorgiaTechLibrary.Services.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace GeorgiaTechLibraryTest.IntegrationTests
{
    public class BookInstanceControllerTest
    {
        private readonly Mock<IBookInstanceService> _bookInstanceServiceMock;
        private readonly BookInstanceController _bookInstanceController;
        private BookInstance _bookInstance;
        private List<BookInstance> _bookInstances;
        private int _id;

        public BookInstanceControllerTest()
        {
            _bookInstanceServiceMock = new Mock<IBookInstanceService>();
            _bookInstanceController = new BookInstanceController(_bookInstanceServiceMock.Object);
        }

        private void SetUp()
        {
            _id = 1;

            var book = new Book
            {
                ISBN = "1",
                Authors = new List<string> { "Author Name" }
            };

            _bookInstances = new List<BookInstance>
            {
                new BookInstance
                {
                    Id = 1,
                    IsLoaned = false,
                    Book = book
                },
                new BookInstance
                {
                    Id = 2,
                    IsLoaned = false,
                    Book = book
                }
            };

            _bookInstance = new BookInstance
            {
                Id = _id,
                IsLoaned = false,
                Book = book
            };
        }

        [Fact]
        public async Task GetBookInstanceAsync_ReturnsOkResult_WithBookInstance()
        {
            // Arrange
            SetUp();
            _bookInstanceServiceMock.Setup(x => x.GetBookInstance(_id)).ReturnsAsync(_bookInstance);

            // Act
            IActionResult result = await _bookInstanceController.GetBookInstanceAsync(_id);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(_bookInstance, okResult.Value);
        }

        [Fact]
        public async Task GetBookInstanceAsync_ReturnsNotFound_WhenBookInstanceNotExists()
        {
            // Arrange
            SetUp();
            _bookInstanceServiceMock.Setup(x => x.GetBookInstance(_id)).ReturnsAsync((BookInstance)null);

            // Act
            IActionResult result = await _bookInstanceController.GetBookInstanceAsync(_id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task ListBookInstancesAsync_ReturnsOkResult_WithBookInstances()
        {
            // Arrange
            SetUp();
            _bookInstanceServiceMock.Setup(x => x.ListBookInstances()).ReturnsAsync(_bookInstances);

            // Act
            IActionResult result = await _bookInstanceController.ListBookInstancesAsync();

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(_bookInstances, okResult.Value);
        }

        [Fact]
        public async Task CreateBookInstance_ReturnsOkResult_WithCreatedBookInstance()
        {
            // Arrange
            SetUp();
            _bookInstanceServiceMock.Setup(x => x.CreateBookInstance(_bookInstance)).ReturnsAsync(_bookInstance);

            // Act
            IActionResult result = await _bookInstanceController.CreateBookInstance(_bookInstance);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(_bookInstance, okResult.Value);
        }

        [Fact]
        public async Task UpdateBookInstance_ReturnsOkResult_WithUpdatedBookInstance()
        {
            // Arrange
            SetUp();
            _bookInstance.IsLoaned = true;
            _bookInstanceServiceMock.Setup(x => x.UpdateBookInstance(_bookInstance)).ReturnsAsync(_bookInstance);

            // Act
            IActionResult result = await _bookInstanceController.UpdateBookInstance(_bookInstance);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(_bookInstance, okResult.Value);
        }

        [Fact]
        public async Task DeleteBookInstance_ReturnsOkResult_WithDeleteSuccess()
        {
            // Arrange
            SetUp();
            _bookInstanceServiceMock.Setup(x => x.DeleteBookInstance(_id)).ReturnsAsync(true);

            // Act
            IActionResult result = await _bookInstanceController.DeleteBookInstance(_id);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(true, okResult.Value);
        }
    }
}
