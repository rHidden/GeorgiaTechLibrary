using GeorgiaTechLibrary.Controllers;
using GeorgiaTechLibrary.Services.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using DataAccess.Models;

namespace GeorgiaTechLibraryTest.IntegrationTests
{
    public class BookControllerTests
    {
        private readonly Mock<IBookService> _bookServiceMock;
        private readonly BookController _bookController;
        private Book _book;
        private List<Book> _books;
        private string _isbn;

        public BookControllerTests()
        {
            _bookServiceMock = new Mock<IBookService>();
            _bookController = new BookController(_bookServiceMock.Object);
        }

        private void SetUp()
        {
            _isbn = "1234567890";

            _books = new List<Book>
            {
                new Book
                {
                    ISBN = "1234567890",
                    SubjectArea = "Subject area 1",
                    Status = "loanable",
                    Description = "Description 1"
                },
                new Book
                {
                    ISBN = "0987654321",
                    SubjectArea = "Subject area 2",
                    Status = "loanable",
                    Description = "Description 2"
                }
            };

            _book = new Book
            {
                ISBN = _isbn,
                SubjectArea = "Subject area",
                Status = "loanable",
                Description = "Description"
            };
        }

        [Fact]
        public async Task GetBookAsync_ReturnsOkResult_WithBook()
        {
            // Arrange
            SetUp();
            _bookServiceMock.Setup(x => x.GetBook(_isbn)).ReturnsAsync(_book);

            // Act
            IActionResult result = await _bookController.GetBookAsync(_isbn);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(_book, okResult.Value);
        }

        [Fact]
        public async Task GetBookAsync_ReturnsNotFound_WhenBookNotExists()
        {
            // Arrange
            SetUp();
            _bookServiceMock.Setup(x => x.GetBook(_isbn)).ReturnsAsync((Book)null);

            // Act
            IActionResult result = await _bookController.GetBookAsync(_isbn);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task ListBooksAsync_ReturnsOkResult_WithBooks()
        {
            // Arrange
            SetUp();
            _bookServiceMock.Setup(x => x.ListBooks()).ReturnsAsync(_books);

            // Act
            IActionResult result = await _bookController.ListBooksAsync();

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(_books, okResult.Value);
        }

        [Fact]
        public async Task CreateBook_ReturnsOkResult_WithCreatedBook()
        {
            // Arrange
            SetUp();
            _bookServiceMock.Setup(x => x.CreateBook(_book)).ReturnsAsync(_book);

            // Act
            IActionResult result = await _bookController.CreateBook(_book);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(_book, okResult.Value);
        }

        [Fact]
        public async Task UpdateBook_ReturnsOkResult_WithUpdatedBook()
        {
            // Arrange
            SetUp();
            _book.Description = "Updated Title";
            _bookServiceMock.Setup(x => x.UpdateBook(_book)).ReturnsAsync(_book);

            // Act
            IActionResult result = await _bookController.UpdateBook(_book);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(_book, okResult.Value);
        }

        [Fact]
        public async Task DeleteBook_ReturnsOkResult_WithDeleteSuccess()
        {
            // Arrange
            SetUp();
            _bookServiceMock.Setup(x => x.DeleteBook(_isbn)).ReturnsAsync(true);

            // Act
            IActionResult result = await _bookController.DeleteBook(_isbn);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(true, okResult.Value);
        }
    }
}
