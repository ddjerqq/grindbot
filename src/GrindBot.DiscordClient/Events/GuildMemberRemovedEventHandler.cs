using DSharpPlus;
using DSharpPlus.EventArgs;

namespace GrindBot.DiscordClient.Events;

public sealed class GuildMemberRemovedEventHandler : IEventHandler<GuildMemberRemovedEventArgs>
{
    private static readonly ulong GoodbyeChannelId = ulong.Parse("GOODBYE_CHANNEL_ID".FromEnv());

    public async Task HandleEventAsync(DSharpPlus.DiscordClient bot, GuildMemberRemovedEventArgs ctx)
    {
        var channel = await bot.GetChannelAsync(GoodbyeChannelId);
        await channel.SendMessageAsync($"Goodbye, {ctx.Member.Mention}!");
    }
}