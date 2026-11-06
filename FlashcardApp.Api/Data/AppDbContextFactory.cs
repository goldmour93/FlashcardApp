using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FlashcardApp.Api.Data;

public class AppDbContextFactory : IDesignTimeDbContextFactory<FlashcardAppDbContext>
{
    public FlashcardAppDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var connectionString = configuration.GetConnectionString("Default");

        var optionsBuilder = new DbContextOptionsBuilder<FlashcardAppDbContext>();
        optionsBuilder.UseNpgsql(connectionString ?? "Host=localhost;Database=flashcardapp;Username=flashcardapp;Password=flashcardapp");

        return new FlashcardAppDbContext(optionsBuilder.Options);
    }
}

