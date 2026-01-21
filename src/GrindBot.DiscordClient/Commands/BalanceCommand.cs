using System.ComponentModel;
using DSharpPlus.Commands;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Entities;
using GrindBot.DiscordClient.Services;

namespace GrindBot.DiscordClient.Commands;

public sealed class BalanceCommand(UserService userService)
{
    [Command("balance")]
    [Description("Gets your or another user's balance")]
    public async ValueTask ExecuteAsync(SlashCommandContext context, [Description("The user whose balance you want to access")] DiscordUser? member = null)
    {
        var userId = member?.Id ?? context.User.Id;
        await userService.EnsureUserExistsAsync(userId);
        var user = await userService.GetUser(userId);
        if (user is null)
        {
            await context.RespondAsync("An error occurred while retrieving the user data.");
            return;
        }
        
        var embed = new DiscordEmbedBuilder()
            .WithColor(DiscordColor.White)
            .WithTitle("💰 User balance")
            .WithDescription($"Balance for {(member ?? context.User).Mention}")
            .AddField("Money", user.Balance.ToString(), true);

        await context.RespondAsync(embed);
    }
}