
using System.Text.Json;
using System.Text.Json.Serialization;

using FlashcardApp.Core.Interfaces;
using FlashcardApp.Core.Models;

namespace FlashcardApp.Core.Repositories
{
    /// <summary>
    /// A simple JSON file-backed repository for storing users and their flashcard decks.
    /// 
    /// This is a good default persistence option for a small desktop app:
    /// - No external services required
    /// - Easy to back up/copy
    /// - Easy to integration test (real file I/O)
    /// </summary>
    public class JsonFileUserRepository : IUserRepository
    {
        private readonly string _filePath;
        private readonly SemaphoreSlim _gate = new(1, 1);

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            IncludeFields = true,
            Converters = { new FlashcardJsonConverter() }
        };

        public JsonFileUserRepository(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("File path must not be null/empty.", nameof(filePath));

            _filePath = filePath;
        }

        public async Task<User?> GetUserByIdAsync(Guid id)
        {
            var users = await ReadAllAsync();
            return users.FirstOrDefault(u => u.Id == id);
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return null;

            var users = await ReadAllAsync();
            return users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        }

        public async Task SaveUserAsync(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            await _gate.WaitAsync();
            try
            {
                var users = await ReadAllUnsafeAsync();

                // Ensure stable id
                if (user.Id == Guid.Empty)
                    user.Id = Guid.NewGuid();

                var existingIndex = users.FindIndex(u => u.Id == user.Id);
                if (existingIndex >= 0)
                {
                    users[existingIndex] = user;
                }
                else
                {
                    users.Add(user);
                }

                await WriteAllUnsafeAsync(users);
            }
            finally
            {
                _gate.Release();
            }
        }

        private async Task<List<User>> ReadAllAsync()
        {
            await _gate.WaitAsync();
            try
            {
                return await ReadAllUnsafeAsync();
            }
            finally
            {
                _gate.Release();
            }
        }

        private async Task<List<User>> ReadAllUnsafeAsync()
        {
            if (!File.Exists(_filePath))
                return [];

            var json = await File.ReadAllTextAsync(_filePath);
            if (string.IsNullOrWhiteSpace(json))
                return [];

            try
            {
                return JsonSerializer.Deserialize<List<User>>(json, JsonOptions) ?? [];
            }
            catch
            {
                // If the file is corrupted/unreadable, fail safe with an empty store.
                // (Alternatively you could throw; for coursework simplicity we keep it resilient.)
                return [];
            }
        }

        private async Task WriteAllUnsafeAsync(List<User> users)
        {
            var dir = Path.GetDirectoryName(_filePath);
            if (!string.IsNullOrWhiteSpace(dir))
                Directory.CreateDirectory(dir);

            var json = JsonSerializer.Serialize(users, JsonOptions);

            // Atomic-ish write: write temp then replace
            var tempPath = _filePath + ".tmp";
            await File.WriteAllTextAsync(tempPath, json);

            if (File.Exists(_filePath))
                File.Delete(_filePath);

            File.Move(tempPath, _filePath);
        }

        /// <summary>
        /// Serialize flashcards without FsrsCard to keep JSON storage simple and stable.
        /// FsrsCard will be re-initialized to a default Card on load.
        /// </summary>
        private sealed class FlashcardJsonConverter : JsonConverter<Flashcard>
        {
            public override Flashcard Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                using var doc = JsonDocument.ParseValue(ref reader);
                var root = doc.RootElement;

                var card = new Flashcard
                {
                    Id = root.TryGetProperty("Id", out var idProp) && idProp.ValueKind == JsonValueKind.String
                        ? Guid.Parse(idProp.GetString() ?? Guid.Empty.ToString())
                        : Guid.Empty,
                    Front = root.TryGetProperty("Front", out var frontProp) ? frontProp.GetString() ?? string.Empty : string.Empty,
                    Back = root.TryGetProperty("Back", out var backProp) ? backProp.GetString() ?? string.Empty : string.Empty,
                    Topic = root.TryGetProperty("Topic", out var topicProp) ? topicProp.GetString() ?? string.Empty : string.Empty,
                    FsrsCard = new FSRS.Core.Models.Card()
                };

                if (card.Id == Guid.Empty)
                {
                    card.Id = Guid.NewGuid();
                }

                return card;
            }

            public override void Write(Utf8JsonWriter writer, Flashcard value, JsonSerializerOptions options)
            {
                writer.WriteStartObject();
                writer.WriteString("Id", value.Id);
                writer.WriteString("Front", value.Front);
                writer.WriteString("Back", value.Back);
                writer.WriteString("Topic", value.Topic);
                writer.WriteEndObject();
            }
        }
    }
}
