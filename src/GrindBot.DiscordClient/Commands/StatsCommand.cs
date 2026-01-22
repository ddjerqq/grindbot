using System.ComponentModel;
using DSharpPlus.Commands;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Entities;
using GrindBot.Application.Services;

namespace GrindBot.DiscordClient.Commands;

public sealed class StatsCommand(UserService userService)
{
    [Command("stats")]
    [Description("Gets your or another user's stats")]
    public async ValueTask ExecuteAsync(SlashCommandContext context, [Description("The user whose stats you want to access")] DiscordUser? member = null)
    {
        var userId = member?.Id ?? context.User.Id;
        var user = await userService.GetUser(userId);
        var invoker = member ?? context.User;

        var embed = new DiscordEmbedBuilder()
            .WithColor(DiscordColor.White)
            .WithTitle("📊 Stats")
            .WithDescription($"Stats for {invoker.Mention}")
            .WithAuthor(invoker.Username, invoker.AvatarUrl, invoker.AvatarUrl)
            .AddField("XP", $"{user.Xp}")
            .AddField("Level", $"{user.Level}")
            .AddField("Balance", $"${user.Balance}");

        await context.RespondAsync(embed);
    }
}