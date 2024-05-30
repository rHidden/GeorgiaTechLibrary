using DataAccess.Models;
using Microsoft.Data.SqlClient;
using Moq;
using DataAccess.Repositories;
using DataAccess.DAO.DAOIntefaces;

namespace GeorgiaTechLibraryTest.UnitTests
{
    public class DigitalItemRepositoryTest
    {
        private Mock<IDatabaseConnectionFactory> _mockDatabaseConnectionFactory;
        private DigitalItemRepository _digitalItemRepository;
        private string _connectionString;

        public DigitalItemRepositoryTest()
        {
            _connectionString = DatabaseConnectionTest._connectionString;
            _mockDatabaseConnectionFactory = new Mock<IDatabaseConnectionFactory>();
            _mockDatabaseConnectionFactory.Setup(d => d.CreateConnection())
                .Returns(() => new SqlConnection(_connectionString));
            _digitalItemRepository = new DigitalItemRepository(_mockDatabaseConnectionFactory.Object);
        }

        [Fact]
        public async Task CreateAudio_CreatesNewAudio()
        {
            // Arrange
            var newAudio = new Audio
            {
                Id = 7,
                Name = "Test Audio",
                Format = "mp3",
                Size = 5.0,
                Length = 300,
                Authors = new List<string> { "Author1" }
            };

            // Act
            var result = await _digitalItemRepository.CreateAudio(newAudio);
            await _digitalItemRepository.DeleteDigitalItem(newAudio.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(newAudio.Id, result.Id);
            Assert.Equal(newAudio.Name, result.Name);
            Assert.Equal(newAudio.Format, result.Format);
            Assert.Equal(newAudio.Size, result.Size);
            Assert.Equal(newAudio.Length, result.Length);
            Assert.Equal(newAudio.Authors, result.Authors);
        }

        [Fact]
        public async Task GetDigitalItem_WithValidId_ReturnsDigitalItem()
        {
            // Arrange
            var newText = new Text
            {
                Id = 8,
                Name = "Test Text",
                Format = "pdf",
                Size = 1.0,
                Authors = new List<string> { "Author1" }
            };

            // Act
            await _digitalItemRepository.CreateText(newText);
            var result = await _digitalItemRepository.GetDigitalItem(newText.Id);
            await _digitalItemRepository.DeleteDigitalItem(newText.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(newText.Id, result.Id);
            Assert.Equal(newText.Name, result.Name);
            Assert.Equal(newText.Format, result.Format);
            Assert.Equal(newText.Size, result.Size);
            Assert.Equal(newText.Authors, result.Authors);
        }

        [Fact]
        public async Task ListDigitalItems_ReturnsListOfDigitalItems()
        {
            // Arrange
            var newText = new Text
            {
                Id = 9,
                Name = "Test Text",
                Format = "pdf",
                Size = 1.0,
                Authors = new List<string> { "Author1" }
            };

            var newVideo = new Video
            {
                Id = 10,
                Name = "Test Video",
                Format = "mp4",
                Size = 100.0,
                Length = 1200,
                Resolution = new Resolution { Width = 1920, Height = 1080 },
                Authors = new List<string> { "Author2" }
            };

            // Act
            await _digitalItemRepository.CreateText(newText);
            await _digitalItemRepository.CreateVideo(newVideo);
            var result = await _digitalItemRepository.ListDigitalItems();
            await _digitalItemRepository.DeleteDigitalItem(newText.Id);
            await _digitalItemRepository.DeleteDigitalItem(newVideo.Id);

            // Assert
            var actualText = result.FirstOrDefault(r => r.Id == newText.Id) as Text;
            var actualVideo = result.FirstOrDefault(r => r.Id == newVideo.Id) as Video;

            Assert.NotNull(actualText);
            Assert.NotNull(actualVideo);

            Assert.Equal(newText.Name, actualText.Name);
            Assert.Equal(newText.Format, actualText.Format);
            Assert.Equal(newText.Size, actualText.Size);
            Assert.Equal(newText.Authors.OrderBy(a => a), actualText.Authors.OrderBy(a => a));

            Assert.Equal(newVideo.Name, actualVideo.Name);
            Assert.Equal(newVideo.Format, actualVideo.Format);
            Assert.Equal(newVideo.Size, actualVideo.Size);
            Assert.Equal(newVideo.Length, actualVideo.Length);
            Assert.Equal(newVideo.Authors.OrderBy(a => a), actualVideo.Authors.OrderBy(a => a));
        }

        [Fact]
        public async Task UpdateDigitalItem_UpdatesExistingDigitalItem()
        {
            // Arrange
            var newText = new Text
            {
                Id = 11,
                Name = "Test Text",
                Format = "pdf",
                Size = 1.0,
                Authors = new List<string> { "Author1" }
            };

            var updatedText = new Text
            {
                Id = 11,
                Name = "Updated Text",
                Format = "epub",
                Size = 1.5,
                Authors = new List<string> { "Author1", "Author2" }
            };

            // Act
            await _digitalItemRepository.CreateText(newText);
            var result = await _digitalItemRepository.UpdateDigitalItem(updatedText);
            await _digitalItemRepository.DeleteDigitalItem(newText.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(updatedText.Id, result.Id);
            Assert.Equal(updatedText.Name, result.Name);
            Assert.Equal(updatedText.Format, result.Format);
            Assert.Equal(updatedText.Size, result.Size);
            Assert.Equal(updatedText.Authors, result.Authors);
        }

        [Fact]
        public async Task DeleteDigitalItem_RemovesDigitalItemFromDatabase()
        {
            // Arrange
            var newText = new Text
            {
                Id = 12,
                Name = "Test Text",
                Format = "pdf",
                Size = 1.0,
                Authors = new List<string> { "Author1" }
            };

            // Act
            await _digitalItemRepository.CreateText(newText);
            var result = await _digitalItemRepository.DeleteDigitalItem(newText.Id);

            // Assert
            Assert.True(result);
        }
    }
}
