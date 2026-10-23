
# Stage 1: Build and test
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /app

COPY . .

# Restore dependencies
RUN dotnet restore FlashcardApp.Tests/FlashcardApp.Tests.csproj

# Run tests - fail the build if tests fail
RUN dotnet test FlashcardApp.Tests/FlashcardApp.Tests.csproj \
    --configuration Release \
    --no-restore

# Publish the runnable API
RUN dotnet publish FlashcardApp.Api/FlashcardApp.Api.csproj \
    --configuration Release \
    --output /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime

WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "FlashcardApp.Api.dll"]
