
using FlashcardApp.Core.Models;
using FlashcardApp.Core.Repositories;


namespace FlashcardApp.Tests.Integration
{
    public class JsonFileUserRepositoryIntegrationTests
    {
        [Fact]
        public async Task SaveUserAsync_And_GetUserByUsernameAsync_RoundTrips_UserAndDeck()
        {
            var tempDir = Path.Combine(Path.GetTempPath(), "FlashcardApp.Tests", Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            var filePath = Path.Combine(tempDir, "users.json");
            var repo = new JsonFileUserRepository(filePath);

            var username = $"it_{Guid.NewGuid():N}";
            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = username,
                TotalXp = 123,
                DailyLimit = 7,
                DesiredRetention = 0.9,
                TopicXp = new Dictionary<string, int> { ["Integration"] = 50 },
                Deck =
                [
                    new() {
                        Id = Guid.NewGuid(),
                        Front = "Q",
                        Back = "A",
                        Topic = "Integration",
                        FsrsCard = new FSRS.Core.Models.Card()
                    }
                ]
            };

            await repo.SaveUserAsync(user);

            var loaded = await repo.GetUserByUsernameAsync(username);

            Assert.NotNull(loaded);
            Assert.Equal(user.Id, loaded!.Id);
            Assert.Equal(user.Username, loaded.Username);
            Assert.Equal(user.TotalXp, loaded.TotalXp);
            Assert.Equal(user.DailyLimit, loaded.DailyLimit);
            Assert.Equal(user.DesiredRetention, loaded.DesiredRetention);

            Assert.NotNull(loaded.TopicXp);
            Assert.True(loaded.TopicXp.ContainsKey("Integration"));
            Assert.Equal(50, loaded.TopicXp["Integration"]);

            Assert.NotNull(loaded.Deck);
            Assert.Single(loaded.Deck);
            Assert.Equal("Q", loaded.Deck[0].Front);
            Assert.Equal("A", loaded.Deck[0].Back);
            Assert.Equal("Integration", loaded.Deck[0].Topic);

            // Re-create repository to ensure it's really persisted to disk (not just in-memory)
            var repo2 = new JsonFileUserRepository(filePath);
            var loaded2 = await repo2.GetUserByUsernameAsync(username);
            Assert.NotNull(loaded2);
            Assert.Equal(user.Username, loaded2!.Username);
        }

        private static string CreateTempFilePath()
        {
            var tempDir = Path.Combine(Path.GetTempPath(), "FlashcardApp.Tests", Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);
            return Path.Combine(tempDir, "users.json");
        }

        [Fact]
        public async Task SaveUserAsync_AssignsId_WhenEmpty()
        {
            var filePath = CreateTempFilePath();
            var repo = new JsonFileUserRepository(filePath);

            var user = new User { Username = $"user_{Guid.NewGuid():N}" };

            await repo.SaveUserAsync(user);

            var loaded = await repo.GetUserByUsernameAsync(user.Username);
            Assert.NotNull(loaded);
            Assert.NotEqual(Guid.Empty, loaded!.Id);
        }

        [Fact]
        public async Task SaveUserAsync_UpdatesExistingUser_ById()
        {
            var filePath = CreateTempFilePath();
            var repo = new JsonFileUserRepository(filePath);

            var user = new User { Id = Guid.NewGuid(), Username = "update_user", TotalXp = 5 };
            await repo.SaveUserAsync(user);

            user.TotalXp = 25;
            await repo.SaveUserAsync(user);

            var loaded = await repo.GetUserByIdAsync(user.Id);
            Assert.NotNull(loaded);
            Assert.Equal(25, loaded!.TotalXp);
        }

        [Fact]
        public async Task GetUserByUsernameAsync_IsCaseInsensitive()
        {
            var filePath = CreateTempFilePath();
            var repo = new JsonFileUserRepository(filePath);

            var user = new User { Id = Guid.NewGuid(), Username = "CaseUser" };
            await repo.SaveUserAsync(user);

            var loaded = await repo.GetUserByUsernameAsync("caseuser");
            Assert.NotNull(loaded);
            Assert.Equal(user.Id, loaded!.Id);
        }

        [Fact]
        public async Task GetUserByUsernameAsync_Whitespace_ReturnsNull()
        {
            var filePath = CreateTempFilePath();
            var repo = new JsonFileUserRepository(filePath);

            var loaded = await repo.GetUserByUsernameAsync("   ");
            Assert.Null(loaded);
        }

        [Fact]
        public async Task CorruptedJsonFile_ReturnsNull_ForLookup()
        {
            var filePath = CreateTempFilePath();
            await File.WriteAllTextAsync(filePath, "{ not: valid json }");

            var repo = new JsonFileUserRepository(filePath);
            var loaded = await repo.GetUserByUsernameAsync("any");

            Assert.Null(loaded);
        }

        [Fact]
        public async Task GetUserByIdAsync_ReturnsUser_WhenPersisted()
        {
            var filePath = CreateTempFilePath();
            var repo = new JsonFileUserRepository(filePath);

            var user = new User { Id = Guid.NewGuid(), Username = "by_id_user" };
            await repo.SaveUserAsync(user);

            var loaded = await repo.GetUserByIdAsync(user.Id);

            Assert.NotNull(loaded);
            Assert.Equal(user.Username, loaded!.Username);
        }

        [Fact]
        public async Task EmptyJsonFile_ReturnsNull_ForLookup()
        {
            var filePath = CreateTempFilePath();
            await File.WriteAllTextAsync(filePath, string.Empty);

            var repo = new JsonFileUserRepository(filePath);
            var loaded = await repo.GetUserByUsernameAsync("any");

            Assert.Null(loaded);
        }

        [Fact]
        public async Task SaveUserAsync_NullUser_ThrowsArgumentNullException()
        {
            var filePath = CreateTempFilePath();
            var repo = new JsonFileUserRepository(filePath);

            await Assert.ThrowsAsync<ArgumentNullException>(() => repo.SaveUserAsync(null!));
        }

        [Fact]
        public async Task GetUserByUsernameAsync_FileDoesNotExist_ReturnsNull()
        {
            var filePath = CreateTempFilePath();
            if (File.Exists(filePath)) File.Delete(filePath);

            var repo = new JsonFileUserRepository(filePath);
            var loaded = await repo.GetUserByUsernameAsync("missing");

            Assert.Null(loaded);
        }

        [Fact]
        public async Task GetUserByIdAsync_FileDoesNotExist_ReturnsNull()
        {
            var filePath = CreateTempFilePath();
            if (File.Exists(filePath)) File.Delete(filePath);

            var repo = new JsonFileUserRepository(filePath);
            var loaded = await repo.GetUserByIdAsync(Guid.NewGuid());

            Assert.Null(loaded);
        }
    }
}
