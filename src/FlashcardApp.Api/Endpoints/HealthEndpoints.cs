using Npgsql;

namespace FlashcardApp.Api.Endpoints;

public static class HealthEndpoints
{
    public static IEndpointRouteBuilder MapHealthEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/", () => "Flashcard API is running");

        // Readiness is different from liveness: it verifies real database connectivity.
        endpoints.MapGet("/health/ready", async (IConfiguration config, CancellationToken cancellationToken) =>
        {
            var connectionString = config.GetConnectionString("Default");
            if (string.IsNullOrWhiteSpace(connectionString))
                return Results.StatusCode(StatusCodes.Status503ServiceUnavailable);

            try
            {
                await using var connection = new NpgsqlConnection(connectionString);
                await connection.OpenAsync(cancellationToken);
                await using var command = new NpgsqlCommand("SELECT 1", connection);
                var result = await command.ExecuteScalarAsync(cancellationToken);

                return Convert.ToInt32(result) == 1
                    ? Results.Ok(new { status = "ready" })
                    : Results.StatusCode(StatusCodes.Status503ServiceUnavailable);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception)
            {
                // Do not disclose credentials or connection strings in the HTTP response.
                return Results.StatusCode(StatusCodes.Status503ServiceUnavailable);
            }
        });

        return endpoints;
    }
}
