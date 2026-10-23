var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<FlashcardApp.Core.Interfaces.IUserRepository>(sp =>
{
    var connectionString = builder.Configuration.GetConnectionString("Default");

    if (!string.IsNullOrWhiteSpace(connectionString))
    {
        return new FlashcardApp.Core.Repositories.PostgresUserRepository(connectionString);
    }

    var dataDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FlashcardApp");
    var filePath = Path.Combine(dataDir, "users.json");
    return new FlashcardApp.Core.Repositories.JsonFileUserRepository(filePath);
});

builder.Services.AddSingleton<FlashcardApp.Core.Services.UserService>();

var app = builder.Build();

app.MapGet("/", () => "Flashcard API is running");

app.Run();