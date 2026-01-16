using dotenv.net;
using GrindBot.DiscordClient.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace GrindBot.DiscordClient.Persistence;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DiscordDbContext>
{
    public DiscordDbContext CreateDbContext(string[] args)
    {
        var solutionDir = Directory.GetParent(Directory.GetCurrentDirectory())?.Parent;
        DotEnv.Fluent().WithTrimValues().WithEnvFiles($"{solutionDir}/.env").WithOverwriteExistingVars().Load();
        
        var dataSource = "DB_PATH".FromEnv();

        var optionsBuilder = new DbContextOptionsBuilder<DiscordDbContext>();
        optionsBuilder.UseSqlite($"Data Source={dataSource}");
        
        return new DiscordDbContext(optionsBuilder.Options);
    }
}

