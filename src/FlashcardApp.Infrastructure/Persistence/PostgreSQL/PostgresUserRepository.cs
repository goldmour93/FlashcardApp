using System.Text.Json;
using FlashcardApp.Core.Interfaces;
using FlashcardApp.Core.Models;
using Npgsql;

namespace FlashcardApp.Infrastructure.Persistence.PostgreSQL;

public class PostgresUserRepository : IUserRepository
{
    private readonly string _connectionString;
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
    };

    public PostgresUserRepository(string connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new ArgumentException("Connection string must not be null or empty.", nameof(connectionString));

        _connectionString = connectionString;
        EnsureSchema();
    }

    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
            return null;

        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        await using var command = new NpgsqlCommand(
            "SELECT id, username, total_xp, daily_limit, desired_retention, topic_xp, deck FROM users WHERE username = @username LIMIT 1;",
            connection);
        command.Parameters.AddWithValue("username", username);

        await using var reader = await command.ExecuteReaderAsync();
        if (!await reader.ReadAsync())
            return null;

        return MapUser(reader);
    }

    public async Task SaveUserAsync(User user)
    {
        ArgumentNullException.ThrowIfNull(user);

        if (user.Id == Guid.Empty)
            user.Id = Guid.NewGuid();

        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        await using var command = new NpgsqlCommand(
            @"INSERT INTO users (id, username, total_xp, daily_limit, desired_retention, topic_xp, deck)
              VALUES (@id, @username, @totalXp, @dailyLimit, @desiredRetention, @topicXp::jsonb, @deck::jsonb)
              ON CONFLICT (id) DO UPDATE SET
                  username = EXCLUDED.username,
                  total_xp = EXCLUDED.total_xp,
                  daily_limit = EXCLUDED.daily_limit,
                  desired_retention = EXCLUDED.desired_retention,
                  topic_xp = EXCLUDED.topic_xp,
                  deck = EXCLUDED.deck;",
            connection);

        command.Parameters.AddWithValue("id", user.Id);
        command.Parameters.AddWithValue("username", user.Username ?? string.Empty);
        command.Parameters.AddWithValue("totalXp", user.TotalXp);
        command.Parameters.AddWithValue("dailyLimit", user.DailyLimit);
        command.Parameters.AddWithValue("desiredRetention", user.DesiredRetention);
        command.Parameters.AddWithValue("topicXp", JsonSerializer.Serialize(user.TopicXp ?? new Dictionary<string, int>(), JsonOptions));
        command.Parameters.AddWithValue("deck", JsonSerializer.Serialize(user.Deck ?? [], JsonOptions));

        await command.ExecuteNonQueryAsync();
    }

    private void EnsureSchema()
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        using var command = new NpgsqlCommand(
            @"CREATE TABLE IF NOT EXISTS users (
                id UUID PRIMARY KEY,
                username TEXT NOT NULL UNIQUE,
                total_xp INTEGER NOT NULL DEFAULT 0,
                daily_limit INTEGER NOT NULL DEFAULT 20,
                desired_retention DOUBLE PRECISION NOT NULL DEFAULT 0.90,
                topic_xp JSONB NOT NULL DEFAULT '{}'::jsonb,
                deck JSONB NOT NULL DEFAULT '[]'::jsonb
            );",
            connection);

        command.ExecuteNonQuery();
    }

    private static User MapUser(NpgsqlDataReader reader)
    {
        var topicXpJson = reader.IsDBNull(reader.GetOrdinal("topic_xp"))
            ? "{}"
            : reader.GetString(reader.GetOrdinal("topic_xp"));

        var deckJson = reader.IsDBNull(reader.GetOrdinal("deck"))
            ? "[]"
            : reader.GetString(reader.GetOrdinal("deck"));

        var user = new User
        {
            Id = reader.GetGuid(reader.GetOrdinal("id")),
            Username = reader.GetString(reader.GetOrdinal("username")),
            TotalXp = reader.GetInt32(reader.GetOrdinal("total_xp")),
            DailyLimit = reader.GetInt32(reader.GetOrdinal("daily_limit")),
            DesiredRetention = reader.GetDouble(reader.GetOrdinal("desired_retention")),
            TopicXp = string.IsNullOrWhiteSpace(topicXpJson)
                ? new Dictionary<string, int>()
                : JsonSerializer.Deserialize<Dictionary<string, int>>(topicXpJson, JsonOptions) ?? new Dictionary<string, int>(),
            Deck = string.IsNullOrWhiteSpace(deckJson)
                ? []
                : JsonSerializer.Deserialize<List<Flashcard>>(deckJson, JsonOptions) ?? []
        };

        return user;
    }
}

