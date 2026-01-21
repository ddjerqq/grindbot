using GrindBot.Application.Abstractions;
using GrindBot.Application.Services;
using GrindBot.Infrastructure;
using GrindBot.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GrindBot.DiscordClient.Common;

public static class ServiceCollectionExt
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddApplicationServices()
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                var dataSource = "DB_PATH".FromEnv();
                options.UseSqlite($"Data Source={dataSource}");
            });

            services.AddMemoryCache();
            services.AddSingleton<UserService>();
            
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(IAppDbContext).Assembly);
            });

            return services;
        }
    }
}