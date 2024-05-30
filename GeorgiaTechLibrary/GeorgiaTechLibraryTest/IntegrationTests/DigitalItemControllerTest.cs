using DataAccess.Models;
using GeorgiaTechLibrary.Controllers;
using GeorgiaTechLibrary.Services.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace GeorgiaTechLibraryTest.IntegrationTests
{
    public class DigitalItemControllerTest
    {
        private readonly Mock<IDigitalItemService> _digitalItemServiceMock;
        private readonly DigitalItemController _digitalItemController;
        private DigitalItem _digitalItem;
        private List<DigitalItem> _digitalItems;
        private int _id;

        public DigitalItemControllerTest()
        {
            _digitalItemServiceMock = new Mock<IDigitalItemService>();
            _digitalItemController = new DigitalItemController(_digitalItemServiceMock.Object);
        }

        private void SetUp()
        {
            _id = 1;

            _digitalItems = new List<DigitalItem>
            {
                new DigitalItem
                {
                    Id = 1,
                    Name = "Test Audio",
                    Format = "mp3",
                    Size = 5.0,
                    Authors = new List<string> { "Author1" }
                },
                new DigitalItem
                {
                    Id = 2,
                    Name = "Test Text",
                    Format = "pdf",
                    Size = 1.0,
                    Authors = new List<string> { "Author2" }
                }
            };

            _digitalItem = new DigitalItem
            {
                Id = _id,
                Name = "Test Audio",
                Format = "mp3",
                Size = 5.0,
                Authors = new List<string> { "Author1" }
            };
        }

        [Fact]
        public async Task GetDigitalItemAsync_ReturnsOkResult_WithDigitalItem()
        {
            // Arrange
            SetUp();
            _digitalItemServiceMock.Setup(x => x.GetDigitalItem(_id)).ReturnsAsync(_digitalItem);

            // Act
            IActionResult result = await _digitalItemController.GetDigitalItemAsync(_id);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(_digitalItem, okResult.Value);
        }

        [Fact]
        public async Task GetDigitalItemAsync_ReturnsNotFound_WhenDigitalItemNotExists()
        {
            // Arrange
            SetUp();
            _digitalItemServiceMock.Setup(x => x.GetDigitalItem(_id)).ReturnsAsync((DigitalItem)null);

            // Act
            IActionResult result = await _digitalItemController.GetDigitalItemAsync(_id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task ListDigitalItemsAsync_ReturnsOkResult_WithDigitalItems()
        {
            // Arrange
            SetUp();
            _digitalItemServiceMock.Setup(x => x.ListDigitalItems()).ReturnsAsync(_digitalItems);

            // Act
            IActionResult result = await _digitalItemController.ListDigitalItemsAsync();

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(_digitalItems, okResult.Value);
        }

        [Fact]
        public async Task CreateAudio_ReturnsOkResult_WithCreatedAudio()
        {
            // Arrange
            SetUp();
            var audio = _digitalItem as Audio;

            _digitalItemServiceMock.Setup(x => x.CreateAudio(audio)).ReturnsAsync(audio);

            // Act
            IActionResult result = await _digitalItemController.CreateAudio(audio);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(audio, okResult.Value);
        }

        [Fact]
        public async Task CreateImage_ReturnsOkResult_WithCreatedImage()
        {
            // Arrange
            SetUp();
            var image = new Image
            {
                Id = _id,
                Resolution = new Resolution { Width = 1920, Height = 1080 }
            };

            _digitalItemServiceMock.Setup(x => x.CreateImage(image)).ReturnsAsync(image);

            // Act
            IActionResult result = await _digitalItemController.CreateImage(image);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(image, okResult.Value);
        }

        [Fact]
        public async Task CreateText_ReturnsOkResult_WithCreatedText()
        {
            // Arrange
            SetUp();
            var text = new Text
            {
                Id = _id,
                Name = "Test Text",
                Format = "pdf",
                Size = 1.0,
                Authors = new List<string> { "Author2" }
            };
            _digitalItemServiceMock.Setup(x => x.CreateText(text)).ReturnsAsync(text);

            // Act
            IActionResult result = await _digitalItemController.CreateText(text);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(text, okResult.Value);
        }

        [Fact]
        public async Task CreateVideo_ReturnsOkResult_WithCreatedVideo()
        {
            // Arrange
            SetUp();

            var video = new Video
            {
                Id = _id,
                Length = 1200,
                Resolution = new Resolution { Width = 1920, Height = 1080 }
            };

            _digitalItemServiceMock.Setup(x => x.CreateVideo(video)).ReturnsAsync(video);

            // Act
            IActionResult result = await _digitalItemController.CreateVideo(video);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(video, okResult.Value);
        }

        [Fact]
        public async Task UpdateDigitalItem_ReturnsOkResult_WithUpdatedDigitalItem()
        {
            // Arrange
            SetUp();
            _digitalItem.Name = "Updated Name";
            _digitalItemServiceMock.Setup(x => x.UpdateDigitalItem(_digitalItem)).ReturnsAsync(_digitalItem);

            // Act
            IActionResult result = await _digitalItemController.UpdateDigitalItem(_digitalItem);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(_digitalItem, okResult.Value);
        }

        [Fact]
        public async Task DeleteDigitalItem_ReturnsOkResult_WithDeleteSuccess()
        {
            // Arrange
            SetUp();
            _digitalItemServiceMock.Setup(x => x.DeleteDigitalItem(_id)).ReturnsAsync(true);

            // Act
            IActionResult result = await _digitalItemController.DeleteDigitalItem(_id);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(true, okResult.Value);
        }
    }
}
