using FlashcardApp.Api.Endpoints;
using FlashcardApp.Core.Interfaces;
using FlashcardApp.Core.Services;
using FlashcardApp.Infrastructure.Persistence.Json;
using FlashcardApp.Infrastructure.Persistence.PostgreSQL;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IUserRepository>(_ =>
{
    var connectionString = builder.Configuration.GetConnectionString("Default");
    if (!string.IsNullOrWhiteSpace(connectionString))
    {
        return new PostgresUserRepository(connectionString);
    }
    var dataDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FlashcardApp");
    return new JsonFileUserRepository(Path.Combine(dataDir, "users.json"));
});

builder.Services.AddSingleton<UserService>();
var app = builder.Build();
app.MapHealthEndpoints();
app.Run();
