using System.ComponentModel;
using DSharpPlus.Commands;
using DSharpPlus.Commands.Processors.SlashCommands;
using GrindBot.Application.Services;
using GrindBot.Domain.Common;

namespace GrindBot.DiscordClient.Commands;

public sealed class DailyCommand(UserService userService)
{
    [Command("daily")]
    [Description("Gets your daily reward")]
    public async ValueTask ExecuteAsync(SlashCommandContext context)
    {
        var user = await userService.GetUser(context.User.Id);
        if (!user.TryCollectDaily(out var collectNextAt))
        {
            await context.RespondAsync($"You cannot collect your daily reward yet. Next collection available at {collectNextAt.FormatRelative()}.");
            return;
        }

        await context.RespondAsync($"You collected your daily reward!\nBalance: ${user.Balance}");
    }
}