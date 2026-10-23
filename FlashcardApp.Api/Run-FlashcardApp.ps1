# Move to the repository root.
Set-Location $PSScriptRoot
$repoRoot = Split-Path -Parent $PSScriptRoot
Set-Location $repoRoot

$envFile = ".env.dev"
$composeFile = "docker-compose.yml"

Write-Host "Starting Docker services for development..."
docker compose --env-file $envFile -f $composeFile up -d --build
if ($LASTEXITCODE -ne 0) {
    Write-Host "Docker compose failed."
    exit 1
}

Write-Host "Building the WPF desktop app locally..."
dotnet build .\FlashcardApp.WpfUI\FlashcardApp.WpfUI.csproj --configuration Debug
if ($LASTEXITCODE -ne 0) {
    Write-Host "WPF build failed!"
    exit 1
}

$exePath = Join-Path $repoRoot "FlashcardApp.WpfUI\bin\Debug\net9.0-windows\FlashcardApp.WpfUI.exe"
if (-not (Test-Path $exePath)) {
    Write-Host "WPF executable not found at: $exePath"
    exit 1
}

Write-Host "Launching WPF app..."
Start-Process -FilePath $exePath -WorkingDirectory (Split-Path -Parent $exePath)

Write-Host "Dev stack is running."
Write-Host "API: http://localhost:8080"
Write-Host "Postgres: localhost:5432"
