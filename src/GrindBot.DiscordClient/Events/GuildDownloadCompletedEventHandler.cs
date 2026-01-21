using DSharpPlus;
using DSharpPlus.EventArgs;
using GrindBot.Application.Services;
using Serilog;

namespace GrindBot.DiscordClient.Events;

public sealed class GuildDownloadCompletedEventHandler(UserService userService) : IEventHandler<GuildDownloadCompletedEventArgs>
{
    public async Task HandleEventAsync(DSharpPlus.DiscordClient sender, GuildDownloadCompletedEventArgs eventArgs)
    {
        Log.Logger.Information("Guild download completed for {GuildCount} guilds", sender.Guilds.Count);

        foreach (var guild in sender.Guilds.Values)
        {
            await foreach (var member in guild.GetAllMembersAsync())
            {
                var exists = await userService.EnsureUserExistsAsync(member.Id);
                
                if (!exists)
                    Log.Logger.Information("Added user: {UserId} ({Username}#{Discriminator})", member.Id, member.Username, member.Discriminator);
            }
        }

        Log.Logger.Information("Guild download completed for {GuildCount} guilds", sender.Guilds.Count);
        Log.Logger.Information("Logged in as {BotId} <{BotUser}>", sender.CurrentUser.Username, sender.CurrentUser.Id);
    }
}