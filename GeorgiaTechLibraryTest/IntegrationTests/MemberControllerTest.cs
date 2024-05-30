using DataAccess.Models;
using GeorgiaTechLibrary.Controllers;
using GeorgiaTechLibrary.Services.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace GeorgiaTechLibraryTest.IntegrationTests  
{
    public class MemberControllerTest
    {
        private readonly Mock<IMemberService> _memberServiceMock;
        private readonly MemberController _memberController;
        private Member _member;
        private List<Member> _members;
        private string _SSN;

        public MemberControllerTest()
        {
            _memberServiceMock = new Mock<IMemberService>();
            _memberController = new MemberController(_memberServiceMock.Object);
        }

        private void SetUp()
        {
            _SSN = "1";

            _members = new List<Member>
            {
                new Member
                {
                    SSN = "1",
                    FirstName = "John",
                    LastName = "Doe",
                    PhoneNumber = "1234567890",
                    UserAddress = new Address { Street = "Main St", StreetNumber = "1", City = "City", ZipCode = "12345" },
                    CardNumber = "123456",
                    ExpiryDate = DateTime.UtcNow.AddYears(1),
                    Photo = "photo_url",
                    MemberType = "Regular"
                },
                new Member
                {
                    SSN = "2",
                    FirstName = "Jane",
                    LastName = "Doe",
                    PhoneNumber = "0987654321",
                    UserAddress = new Address { Street = "Second St", StreetNumber = "2", City = "City", ZipCode = "54321" },
                    CardNumber = "654321",
                    ExpiryDate = DateTime.UtcNow.AddYears(1),
                    Photo = "photo_url",
                    MemberType = "Premium"
                }
            };

            _member = new Member
            {
                SSN = _SSN,
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "1234567890",
                UserAddress = new Address { Street = "Main St", StreetNumber = "1", City = "City", ZipCode = "12345" },
                CardNumber = "123456",
                ExpiryDate = DateTime.UtcNow.AddYears(1),
                Photo = "photo_url",
                MemberType = "Regular"
            };
        }

        [Fact]
        public async Task GetMember_ReturnsOkResult_WithMember()
        {
            // Arrange
            SetUp();
            _memberServiceMock.Setup(x => x.GetMember(_SSN)).ReturnsAsync(_member);

            // Act
            IActionResult result = await _memberController.GetMember(_SSN);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(_member, okResult.Value);
        }

        [Fact]
        public async Task GetMember_ReturnsNotFound_WhenMemberNotExists()
        {
            // Arrange
            SetUp();
            _memberServiceMock.Setup(x => x.GetMember(_SSN)).ReturnsAsync((Member)null);

            // Act
            IActionResult result = await _memberController.GetMember(_SSN);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task ListMembers_ReturnsOkResult_WithMembers()
        {
            // Arrange
            SetUp();
            _memberServiceMock.Setup(x => x.ListMembers()).ReturnsAsync(_members);

            // Act
            IActionResult result = await _memberController.ListMembers();

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(_members, okResult.Value);
        }

        [Fact]
        public async Task CreateMember_ReturnsOkResult_WithCreatedMember()
        {
            // Arrange
            SetUp();
            _memberServiceMock.Setup(x => x.CreateMember(_member)).ReturnsAsync(_member);

            // Act
            IActionResult result = await _memberController.CreateMember(_member);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(_member, okResult.Value);
        }

        [Fact]
        public async Task UpdateMember_ReturnsOkResult_WithUpdatedMember()
        {
            // Arrange
            SetUp();
            _member.FirstName = "updated name";
            _memberServiceMock.Setup(x => x.UpdateMember(_member)).ReturnsAsync(_member);

            // Act
            IActionResult result = await _memberController.UpdateMember(_member);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(_member, okResult.Value);
        }

        [Fact]
        public async Task DeleteMember_ReturnsOkResult_WithDeleteSuccess()
        {
            // Arrange
            SetUp();
            _memberServiceMock.Setup(x => x.DeleteMember(_SSN)).ReturnsAsync(true);

            // Act
            IActionResult result = await _memberController.DeleteMember(_SSN);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(true, okResult.Value);
        }
    }
}
