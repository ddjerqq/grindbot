using DSharpPlus;
using DSharpPlus.EventArgs;
using GrindBot.Infrastructure;
using GrindBot.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace GrindBot.DiscordClient.Events;

public sealed class SessionCreatedEventHandler(AppDbContext db) : IEventHandler<SessionCreatedEventArgs>
{
    public async Task HandleEventAsync(DSharpPlus.DiscordClient bot, SessionCreatedEventArgs ctx)
    {
        if ((await db.Database.GetPendingMigrationsAsync()).Any())
        {
            var migrations = await db.Database.GetPendingMigrationsAsync();
            Log.Information("Applying migrations: {Migrations}", string.Join(", ", migrations));
            await db.Database.MigrateAsync();
        }

        Log.Information("All migrations applied");

        await db.SaveChangesAsync();
    }
}