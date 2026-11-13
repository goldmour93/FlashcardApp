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

## Week 7 Jenkins CI: Docker build, smoke tests and Docker Hub

The Jenkinsfile retains .NET restore/build, xUnit tests, archived JUnit results and the existing GitHub webhook trigger. After those stages succeed it:

1. Generates a disposable PostgreSQL password in a workspace-only, gitignored `.env`; each build gets its own Compose project.
2. Builds the ASP.NET Core API image from the multi-stage Dockerfile.
3. Starts the API and PostgreSQL in an isolated Compose network without binding host ports, using `docker-compose.ci.yml`.
4. Runs a PostgreSQL `SELECT 1`, checks `GET /`, and verifies `GET /health/ready` (which queries PostgreSQL and returns HTTP 503 if unavailable).
5. On the `main` branch, publishes `goldmour/flashcardapp-api:<short-commit-sha>` to Docker Hub.
6. Always stops its own CI containers, removes their *temporary* volumes and deletes its temporary `.env`.

### Required Jenkins setup

- The Jenkins execution node must have .NET 9 SDK, Docker CLI/Compose and access to a Docker engine. If Jenkins uses the `docker:2376` DinD host, keep the separate `jenkins-docker` container running and reachable.
- Create a Jenkins **Username with password** credential with ID **`dockerhub-flashcard`**. Username: `goldmour`; password: a **fresh Docker Hub personal access token** with repository read/write permission. Never commit or paste the token into the Jenkinsfile.
- Publishing is deliberately limited to builds whose Jenkins Git branch is `origin/main` or `main`. Feature-branch testing skips Docker Hub publishing.
- The Docker agent must be able to pull `postgres:16` and `curlimages/curl:8.16.0`, and reach Docker Hub to publish images.
- This pipeline is designed for a trusted, isolated university Jenkins setup. Docker daemon access is highly privileged. Keep credentials and the DinD TLS connection secure.

The normal developer command remains `docker compose up --build`; use your own local `.env` copied from `.env.example`. The CI overlay is separate from that workflow. Only the API is containerised; the WPF desktop UI is **not** deployed by Jenkins.

### Verify the readiness endpoint

With API and PostgreSQL running, request `GET /health/ready`. A healthy database yields HTTP 200 with `{"status":"ready"}`. Missing or unavailable PostgreSQL configuration yields HTTP 503. The existing root endpoint `GET /` remains a basic liveness check.

### CI implementation notes

The Jenkins stage uses a disposable database and `docker compose down --volumes` in an isolated Compose project. It must never be pointed at a development or production database. If image publishing is unexpectedly skipped on `main`, inspect the `GIT_BRANCH`/ `BRANCH_NAME` values for your Jenkins job type. The change has not been run against the author's Jenkins installation and should be tested on the feature branch before merging.
