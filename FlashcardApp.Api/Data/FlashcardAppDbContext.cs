using System.Text.Json;
using FlashcardApp.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace FlashcardApp.Api.Data;

public class FlashcardAppDbContext : DbContext
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public FlashcardAppDbContext(DbContextOptions<FlashcardAppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Username).IsRequired();
            entity.HasIndex(x => x.Username).IsUnique();
            entity.Property(x => x.TotalXp).HasDefaultValue(0);
            entity.Property(x => x.DailyLimit).HasDefaultValue(20);
            entity.Property(x => x.DesiredRetention).HasDefaultValue(0.90);

            entity.Property(x => x.TopicXp)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, SerializerOptions),
                    v => string.IsNullOrWhiteSpace(v)
                        ? new Dictionary<string, int>()
                        : JsonSerializer.Deserialize<Dictionary<string, int>>(v, SerializerOptions) ?? new Dictionary<string, int>())
                .HasColumnType("jsonb")
                .HasDefaultValueSql("'{}'::jsonb");

            entity.Property(x => x.Deck)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, SerializerOptions),
                    v => string.IsNullOrWhiteSpace(v)
                        ? new List<Flashcard>()
                        : JsonSerializer.Deserialize<List<Flashcard>>(v, SerializerOptions) ?? new List<Flashcard>())
                .HasColumnType("jsonb")
                .HasDefaultValueSql("'[]'::jsonb");
        });
    }
}
