using System.ComponentModel;
using DSharpPlus.Commands;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Entities;
using GrindBot.DiscordClient.Services;

namespace GrindBot.DiscordClient.Commands;

public sealed class DailyCommand(UserService userService)
{
    [Command("daily")]
    [Description("Gets your daily reward")]
    public async ValueTask ExecuteAsync(SlashCommandContext context)
    {
        await userService.EnsureUserExistsAsync(context.User.Id);
        var user = await userService.GetUser(context.User.Id);
        if (user is null)
        {
            await context.RespondAsync("An error occurred while retrieving the user data.");
            return;
        }

        if (!await userService.TryCollectDailyReward(user, out var collectNextAt))
        {
            // get milliseconds as unix epoch
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