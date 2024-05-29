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
    public class StaffControllerTest
    {
        private readonly Mock<IStaffService> _staffServiceMock;
        private readonly StaffController _staffController;
        private Staff _staff;
        private List<Staff> _staffList;
        private string _SSN;

        public StaffControllerTest()
        {
            _staffServiceMock = new Mock<IStaffService>();
            _staffController = new StaffController(_staffServiceMock.Object);
        }

        private void SetUp()
        {
            _SSN = "1";

            _staffList = new List<Staff>
            {
                new Staff
                {
                    SSN = "1",
                    FirstName = "John",
                    LastName = "Doe",
                    PhoneNumber = "1234567890",
                    UserAddress = new Address { Street = "Main St", StreetNumber = "1", City = "City", ZipCode = "12345" },
                    LibrarianNumber = "123456",
                    Role = "Librarian"
                },
                new Staff
                {
                    SSN = "2",
                    FirstName = "Jane",
                    LastName = "Doe",
                    PhoneNumber = "0987654321",
                    UserAddress = new Address { Street = "Second St", StreetNumber = "2", City = "City", ZipCode = "54321" },
                    LibrarianNumber = "654321",
                    Role = "Assistant Librarian"
                }
            };

            _staff = new Staff
            {
                SSN = _SSN,
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "1234567890",
                UserAddress = new Address { Street = "Main St", StreetNumber = "1", City = "City", ZipCode = "12345" },
                LibrarianNumber = "123456",
                Role = "Librarian"
            };
        }

        [Fact]
        public async Task GetStaff_ReturnsOkResult_WithStaff()
        {
            // Arrange
            SetUp();
            _staffServiceMock.Setup(x => x.GetStaff(_SSN)).ReturnsAsync(_staff);

            // Act
            IActionResult result = await _staffController.GetStaff(_SSN);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(_staff, okResult.Value);
        }

        [Fact]
        public async Task GetStaff_ReturnsNotFound_WhenStaffNotExists()
        {
            // Arrange
            SetUp();
            _staffServiceMock.Setup(x => x.GetStaff(_SSN)).ReturnsAsync((Staff)null);

            // Act
            IActionResult result = await _staffController.GetStaff(_SSN);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task ListStaff_ReturnsOkResult_WithStaffList()
        {
            // Arrange
            SetUp();
            _staffServiceMock.Setup(x => x.ListStaff()).ReturnsAsync(_staffList);

            // Act
            IActionResult result = await _staffController.ListStaff();

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(_staffList, okResult.Value);
        }

        [Fact]
        public async Task CreateStaff_ReturnsOkResult_WithCreatedStaff()
        {
            // Arrange
            SetUp();
            _staffServiceMock.Setup(x => x.CreateStaff(_staff)).ReturnsAsync(_staff);

            // Act
            IActionResult result = await _staffController.CreateStaff(_staff);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(_staff, okResult.Value);
        }

        [Fact]
        public async Task UpdateStaff_ReturnsOkResult_WithUpdatedStaff()
        {
            // Arrange
            SetUp();
            _staff.FirstName = "updated name";
            _staffServiceMock.Setup(x => x.UpdateStaff(_staff)).ReturnsAsync(_staff);

            // Act
            IActionResult result = await _staffController.UpdateStaff(_staff);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(_staff, okResult.Value);
        }

        [Fact]
        public async Task DeleteStaff_ReturnsOkResult_WithDeleteSuccess()
        {
            // Arrange
            SetUp();
            _staffServiceMock.Setup(x => x.DeleteStaff(_SSN)).ReturnsAsync(true);

            // Act
            IActionResult result = await _staffController.DeleteStaff(_SSN);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(true, okResult.Value);
        }
    }
}
