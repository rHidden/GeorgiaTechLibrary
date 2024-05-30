using DataAccess.Models;
using GeorgiaTechLibrary.Controllers;
using GeorgiaTechLibrary.DTOs;
using GeorgiaTechLibrary.Services.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace GeorgiaTechLibraryTest.IntegrationTests
{
    public class LoanControllerTest
    {
        private readonly Mock<ILoanService> _loanServiceMock;
        private readonly LoanController _loanController;
        private LoanDTO _loanDTO;
        private List<LoanDTO> _loanDTOList;
        private BookLoan _bookLoan;
        private DigitalItemLoan _digitalItemLoan;

        public LoanControllerTest()
        {
            _loanServiceMock = new Mock<ILoanService>();
            _loanController = new LoanController(_loanServiceMock.Object);
        }

        private void SetUp()
        {
            _loanDTO = new LoanDTO
            {
                Id = 1,
                LoanDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(7),
                ReturnDate = DateTime.Now.AddDays(1),
                User = new UserDTO { SSN = "123456789" },
            };

            _loanDTOList = new List<LoanDTO>
            {
                _loanDTO
            };

            _bookLoan = new BookLoan
            {
                Id = 1,
                LoanDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(7),
                ReturnDate = null,
                User = new User { SSN = "123456789" },
                BookInstance = new BookInstance { Id = 1, IsLoaned = false, Book = new Book() }
            };

            _digitalItemLoan = new DigitalItemLoan
            {
                Id = 2,
                LoanDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(7),
                ReturnDate = null,
                User = new User { SSN = "987654321" },
                DigitalItem = new DigitalItem { Id = 1, Format = "PDF", Size = 2.5 }
            };
        }

        [Fact]
        public async Task GetLoan_ReturnsOkResult_WithLoan()
        {
            // Arrange
            SetUp();
            _loanServiceMock.Setup(x => x.GetLoan(It.IsAny<int>())).ReturnsAsync(_loanDTO);

            // Act
            IActionResult result = await _loanController.GetLoan(_loanDTO.Id);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(_loanDTO, okResult.Value);
        }

        [Fact]
        public async Task GetLoan_ReturnsNotFound_WhenLoanNotExists()
        {
            // Arrange
            SetUp();
            _loanServiceMock.Setup(x => x.GetLoan(It.IsAny<int>())).ReturnsAsync((LoanDTO)null);

            // Act
            IActionResult result = await _loanController.GetLoan(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task ListUserLoans_ReturnsOkResult_WithLoanList()
        {
            // Arrange
            SetUp();
            _loanServiceMock.Setup(x => x.ListUserLoans(It.IsAny<string>())).ReturnsAsync(_loanDTOList);

            // Act
            IActionResult result = await _loanController.ListUserLoans("123456789");

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(_loanDTOList, okResult.Value);
        }

        [Fact]
        public async Task CreateLoan_Book_ReturnsOkResult_WithCreatedLoan()
        {
            // Arrange
            SetUp();
            _loanServiceMock.Setup(x => x.CreateLoan(_bookLoan)).ReturnsAsync(_bookLoan);

            // Act
            IActionResult result = await _loanController.CreateLoan(_bookLoan);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(_bookLoan, okResult.Value);
        }

        [Fact]
        public async Task CreateLoan_DigitalItem_ReturnsOkResult_WithCreatedLoan()
        {
            // Arrange
            SetUp();
            _loanServiceMock.Setup(x => x.CreateLoan(_digitalItemLoan)).ReturnsAsync(_digitalItemLoan);

            // Act
            IActionResult result = await _loanController.CreateLoan(_digitalItemLoan);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(_digitalItemLoan, okResult.Value);
        }

        [Fact]
        public async Task UpdateLoan_ReturnsOkResult_WithUpdatedLoan()
        {
            // Arrange
            SetUp();
            var loanToUpdate = new Loan { Id = 1, ReturnDate = DateTime.Now };
            _loanServiceMock.Setup(x => x.UpdateLoan(It.IsAny<Loan>())).ReturnsAsync(loanToUpdate);

            // Act
            IActionResult result = await _loanController.UpdateLoan(loanToUpdate);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(loanToUpdate, okResult.Value);
        }

        [Fact]
        public async Task DeleteLoan_ReturnsOkResult_WithDeleteSuccess()
        {
            // Arrange
            SetUp();
            _loanServiceMock.Setup(x => x.DeleteLoan(It.IsAny<int>())).ReturnsAsync(true);

            // Act
            IActionResult result = await _loanController.DeleteLoan(1);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(true, okResult.Value);
        }

        [Fact]
        public async Task DeleteLoan_ReturnsOkResult_WithDeleteFailure()
        {
            // Arrange
            SetUp();
            _loanServiceMock.Setup(x => x.DeleteLoan(It.IsAny<int>())).ReturnsAsync(false);

            // Act
            IActionResult result = await _loanController.DeleteLoan(1);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(false, okResult.Value);
        }
    }
}
