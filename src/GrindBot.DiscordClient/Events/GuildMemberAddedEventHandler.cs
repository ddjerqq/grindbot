using DSharpPlus;
using DSharpPlus.EventArgs;

namespace GrindBot.DiscordClient.Events;

public sealed class GuildMemberAddedEventHandler : IEventHandler<GuildMemberAddedEventArgs>
{
    public async Task HandleEventAsync(DSharpPlus.DiscordClient bot, GuildMemberAddedEventArgs ctx)
    {
        throw new NotImplementedException();
    }
}