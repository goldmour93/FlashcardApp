namespace FlashcardApp.Api.Endpoints;

public static class HealthEndpoints
{
    public static IEndpointRouteBuilder MapHealthEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/", () => "Flashcard API is running");
        return endpoints;
    }
}
