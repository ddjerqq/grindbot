using GrindBot.Application.Abstractions;
using GrindBot.Application.Services;
using GrindBot.Domain.Common;
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
                var dataSource = "ECONOMY_DB_PATH".FromEnv();
                options.UseSqlite($"Data Source={dataSource}");
            });
            services.AddScoped<IAppDbContext, AppDbContext>();

            services.AddMemoryCache();
            services.AddSingleton<UserService>();

            services.AddMediatR(cfg => { cfg.RegisterServicesFromAssembly(typeof(IAppDbContext).Assembly); });

            # region samoqalaqo services

            services.AddDbContext<SamoqalaqoDbContext>(options =>
            {
                var dataSource = "SAMOQALAQO_DB_PATH".FromEnv();
                options.UseSqlite($"Data Source={dataSource};Mode=ReadOnly");
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });
            services.AddScoped<ISamoqalaqoRepository, SamoqalaqoDbContext>();
            services.AddScoped<SamoqalaqoService>();

            #endregion

            return services;
        }
    }
}