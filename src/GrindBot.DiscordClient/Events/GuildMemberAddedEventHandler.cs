using DSharpPlus;
using DSharpPlus.EventArgs;
using GrindBot.Application.Services;

namespace GrindBot.DiscordClient.Events;

public sealed class GuildMemberAddedEventHandler(UserService userService) : IEventHandler<GuildMemberAddedEventArgs>
{
    private static readonly ulong WelcomeChannelId = ulong.Parse("WELCOME_CHANNEL_ID".FromEnv());
    
    public async Task HandleEventAsync(DSharpPlus.DiscordClient bot, GuildMemberAddedEventArgs ctx)
    {
        await userService.EnsureUserExistsAsync(ctx.Member.Id);
        
        var channel = await bot.GetChannelAsync(WelcomeChannelId);
        await channel.SendMessageAsync($"Welcome, {ctx.Member.Mention}!");
    }
}