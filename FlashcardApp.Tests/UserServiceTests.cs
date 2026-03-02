
using FlashcardApp.Core.Interfaces;
using FlashcardApp.Core.Models;
using FlashcardApp.Core.Services;
using Moq;


namespace FlashcardApp.Tests
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _mockRepo;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _mockRepo = new Mock<IUserRepository>();
            _userService = new UserService(_mockRepo.Object);
        }

        [Fact]
        public async Task AddXpToUserAsync_ValidXp_UpdatesTotalAndTopicXp()
        {
            // Arrange
            var user = new User { Username = "Callum", TotalXp = 100 };
            string topic = "C# and .Net";
            int xpGained = 15;

            // Act
            await _userService.AddXpToUserAsync(user, topic, xpGained);

            // Assert
            Assert.Equal(115, user.TotalXp);
            Assert.True(user.TopicXp.ContainsKey(topic));
            Assert.Equal(15, user.TopicXp[topic]);
            
            // Verify that SaveUserAsync was called exactly once
            _mockRepo.Verify(repo => repo.SaveUserAsync(user), Times.Once);
        }

        [Fact]
        public async Task AddXpToUserAsync_NegativeXp_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            var user = new User { Username = "Callum", TotalXp = 100 };
            string topic = "Clean Architecture";
            int xpGained = -5; // Invalid XP

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => _userService.AddXpToUserAsync(user, topic, xpGained));
            
            // Verify that SaveUserAsync was NEVER called because it threw an exception
            _mockRepo.Verify(repo => repo.SaveUserAsync(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task GetOrCreateUserAsync_EmptyUsername_ThrowsArgumentException()
        {
            await Assert.ThrowsAsync<ArgumentException>(() => _userService.GetOrCreateUserAsync(""));
            await Assert.ThrowsAsync<ArgumentException>(() => _userService.GetOrCreateUserAsync(null!));
        }

        [Fact]
        public async Task GetOrCreateUserAsync_ExistingUser_ReturnsExistingUser()
        {
            // Arrange
            var existingUser = new User { Username = "ExistingUser" };
            _mockRepo.Setup(r => r.GetUserByUsernameAsync("ExistingUser")).ReturnsAsync(existingUser);

            // Act
            var result = await _userService.GetOrCreateUserAsync("ExistingUser");

            // Assert
            Assert.Equal(existingUser, result);
            _mockRepo.Verify(r => r.SaveUserAsync(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task GetOrCreateUserAsync_NewUser_CreatesAndSavesUser()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetUserByUsernameAsync("NewUser")).ReturnsAsync((User?)null);

            // Act
            var result = await _userService.GetOrCreateUserAsync("NewUser");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("NewUser", result.Username);
            _mockRepo.Verify(r => r.SaveUserAsync(It.IsAny<User>()), Times.Once);
        }
    }
}