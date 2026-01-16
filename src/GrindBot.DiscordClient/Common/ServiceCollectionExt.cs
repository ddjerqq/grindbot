using GrindBot.DiscordClient.Persistence;
using GrindBot.DiscordClient.Services;
using Microsoft.EntityFrameworkCore;

namespace GrindBot.DiscordClient.Common;

public static class ServiceCollectionExt
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddApplicationServices()
        {
            services.AddDbContext<DiscordDbContext>(options =>
            {
                var dataSource = "DB_PATH".FromEnv();
                options.UseSqlite($"Data Source={dataSource}");
            });

            services.AddMemoryCache();
            services.AddSingleton<UserService>();

            return services;
        }
    }
}