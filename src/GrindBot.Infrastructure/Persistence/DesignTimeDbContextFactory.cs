using dotenv.net;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace GrindBot.Infrastructure.Persistence;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var solutionDir = Directory.GetParent(Directory.GetCurrentDirectory())?.Parent;
        DotEnv.Fluent().WithTrimValues().WithEnvFiles($"{solutionDir}/.env").WithOverwriteExistingVars().Load();
        
        var dataSource = "DB_PATH".FromEnv();

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlite($"Data Source={dataSource}");
        
        return new AppDbContext(optionsBuilder.Options);
    }
}

