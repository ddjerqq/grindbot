add-migration:
	dotnet ef migrations add --project ./src/GrindBot.DiscordClient/GrindBot.DiscordClient.csproj --startup-project ./src/GrindBot.DiscordClient/GrindBot.DiscordClient.csproj --context GrindBot.DiscordClient.AppDbContext --configuration Debug --verbose $(name) --output-dir Persistence/Migrations

apply-migrations:
	dotnet ef database update --project ./src/GrindBot.DiscordClient/GrindBot.DiscordClient.csproj --startup-project ./src/GrindBot.DiscordClient/GrindBot.DiscordClient.csproj --context GrindBot.DiscordClient.AppDbContext --configuration Debug --verbose
