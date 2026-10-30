# FlashcardApp

A .NET 9 flashcard app with FSRS scheduling, WPF desktop UI, optional PostgreSQL persistence, and an ASP.NET Core API.

## Layout

- `src/FlashcardApp.Core/` – models, interfaces, domain services and FSRS facade
- `src/FlashcardApp.Infrastructure/` – in-memory, JSON and PostgreSQL repositories
- `src/FlashcardApp.WpfUI/` – WPF views, resources and future view models
- `src/FlashcardApp.Api/` – ASP.NET Core endpoint and composition root
- `tests/Unit/` and `tests/Integration/` – xUnit tests (project file at `tests/FlashcardApp.Tests.csproj`)
- `k8s/` – Kustomize base and environment overlays

## Build and test

Requires the .NET 9 SDK; WPF builds/runs on Windows.

```powershell
dotnet restore FlashcardApp.sln
dotnet build FlashcardApp.sln
dotnet test tests/FlashcardApp.Tests.csproj
dotnet run --project src/FlashcardApp.WpfUI/FlashcardApp.WpfUI.csproj
```

Run the API separately with `dotnet run --project src/FlashcardApp.Api/FlashcardApp.Api.csproj`. Its `GET /` route returns a running-status message; it does not yet expose flashcard CRUD operations.

## Docker Compose (API + PostgreSQL)

Copy `.env.example` to `.env`, choose a **local-only** PostgreSQL password and set that same value in both `POSTGRES_PASSWORD` and `DB_CONNECTION_STRING`, then run:

```powershell
Copy-Item .env.example .env
docker compose up --build
```

Windows users can use `src/FlashcardApp.Api/Run-FlashcardApp.ps1` to start Compose, build WPF and launch the desktop UI. Never commit `.env`, real DB connection strings or Kubernetes Secrets.

## Known follow-ups

- WPF still has study-session logic in code-behind; moving this to view models should be a separate behaviour-preserving change.
- The JSON flashcard converter does not currently persist FSRS review state. Restore that persistence before relying on review schedules across restarts.
- Real Kubernetes Secrets must be provisioned separately; see `k8s/README.md`.
