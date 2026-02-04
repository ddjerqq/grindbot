param(
    [string]$Name
)

if ([string]::IsNullOrWhiteSpace($Name)) {
    $Name = Read-Host "Migration name"
}

if ([string]::IsNullOrWhiteSpace($Name)) {
    Write-Error "Migration name is required."
    exit 1
}

dotnet ef migrations add $Name `
    --verbose `
    --project src\GrindBot.Infrastructure\GrindBot.Infrastructure.csproj `
    --startup-project src\GrindBot.DiscordClient\GrindBot.DiscordClient.csproj `
    --context GrindBot.Infrastructure.Persistence.AppDbContext `
    --configuration Debug `
    --output-dir Persistence/Migrations
