add-migration:
	dotnet ef migrations add $(name) --project ./src/GrindBot.DiscordClient/GrindBot.DiscordClient.csproj --startup-project ./src/GrindBot.DiscordClient/GrindBot.DiscordClient.csproj --context GrindBot.DiscordClient.Persistence.DiscordDbContext --configuration Debug --verbose --output-dir Persistence/Migrations
