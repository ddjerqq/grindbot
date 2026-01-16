using System.ComponentModel;
using DSharpPlus.Commands;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Entities;
using GrindBot.DiscordClient.Services;

namespace GrindBot.DiscordClient.Commands;

public sealed class StatsCommand(UserService userService)
{
    [Command("stats")]
    [Description("Gets your or another user's stats")]
    public async ValueTask ExecuteAsync(SlashCommandContext context, [Description("The user whose stats you want to access")] DiscordUser? member = null)
    {
        var userId = member?.Id ?? context.User.Id;
        await userService.EnsureUserExistsAsync(userId);
        var user = await userService.GetUser(userId);
        
        var embed = new DiscordEmbedBuilder()
            .WithColor(DiscordColor.White)
            .WithTitle("📊 User Stats")
            .WithDescription($"Stats for {(member ?? context.User).Mention}")
            .AddField("XP", user?.Xp.ToString() ?? "0", true)
            .AddField("Level", "0", true)
            .WithFooter("Keep chatting and catching stars to earn more XP!");

        await context.RespondAsync(embed);
    }
}