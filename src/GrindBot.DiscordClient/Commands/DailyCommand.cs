using System.ComponentModel;
using DSharpPlus.Commands;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Entities;
using GrindBot.Application.Services;

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
            var nextMs = new DateTimeOffset(collectNextAt.Value).ToUnixTimeMilliseconds();
            await context.RespondAsync($"You cannot collect your daily reward yet. Next collection available at <t:{nextMs}:R>.");
            return;
        }

        await context.RespondAsync($"Balance: ${user.Balance}");
    }
}