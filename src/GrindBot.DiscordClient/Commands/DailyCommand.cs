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
        if (!await userService.TryCollectDailyReward(user, out var collectNextAt))
        {
            var nextMs = new DateTimeOffset(collectNextAt!.Value).ToUnixTimeMilliseconds();
            await context.RespondAsync($"You cannot collect your daily reward yet. Next collection available at <t:{nextMs}:R>.");
            return;
        }
        
        var embed = new DiscordEmbedBuilder()
            .WithColor(DiscordColor.White)
            .WithTitle("💰 User balance")
            .WithDescription($"Balance for {context.User.Mention}")
            .AddField("Money", user.Balance.ToString(), true);

        await context.RespondAsync(embed);
    }
}