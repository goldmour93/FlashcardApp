using FlashcardApp.Core.Models;
using FlashcardApp.Core.Repositories;

namespace FlashcardApp.Tests
{
    public class InMemoryUserRepositoryTests
    {
        private readonly InMemoryUserRepository _sut;

        public InMemoryUserRepositoryTests()
        {
            _sut = new InMemoryUserRepository();
        }

        [Fact]
        public async Task SaveUserAsync_And_GetUserByIdAsync_WorksCorrectly()
        {
            // Arrange
            var user = new User { Id = Guid.NewGuid(), Username = "TestUser" };

            // Act
            await _sut.SaveUserAsync(user);
            var retrievedUser = await _sut.GetUserByIdAsync(user.Id);

            // Assert
            Assert.NotNull(retrievedUser);
            Assert.Equal(user.Id, retrievedUser.Id);
            Assert.Equal(user.Username, retrievedUser.Username);
        }

        [Fact]
        public async Task GetUserByIdAsync_NonExistent_ReturnsNull()
        {
            // Act
            var result = await _sut.GetUserByIdAsync(Guid.NewGuid());

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetUserByUsernameAsync_Existing_ReturnsUser()
        {
            // Arrange
            var user = new User { Id = Guid.NewGuid(), Username = "TestUser" };
            await _sut.SaveUserAsync(user);

            // Act
            var result = await _sut.GetUserByUsernameAsync("TestUser");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(user.Id, result.Id);
        }

        [Fact]
        public async Task GetUserByUsernameAsync_NonExistent_ReturnsNull()
        {
            // Act
            var result = await _sut.GetUserByUsernameAsync("NonExistent");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task SaveUserAsync_NullUser_ThrowsArgumentNullException()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _sut.SaveUserAsync(null!));
        }

        [Fact]
        public async Task GetUserByUsernameAsync_DifferentCasing_ReturnsUser()
        {
            // Arrange
            var user = new User { Id = Guid.NewGuid(), Username = "TestUser" };
            await _sut.SaveUserAsync(user);

            // Act
            var result = await _sut.GetUserByUsernameAsync("testuser");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(user.Id, result!.Id);
        }
    }
}