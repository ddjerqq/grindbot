using System.ComponentModel;
using DSharpPlus.Commands;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Entities;
using GrindBot.Application.Services;

namespace GrindBot.DiscordClient.Commands;

public sealed class GiveCommand(UserService userService)
{
    [Command("give")]
    [Description("Give your money to another user")]
    public async ValueTask ExecuteAsync(
        SlashCommandContext context, 
        [Description("The user who you want to give your credits to")] DiscordUser member,
        [Description("How much do you want to send")] int amount)
    {
        if (amount <= 0)
        {
            await context.RespondAsync("You must give a positive amount of money.");
            return;
        }

        var user = await userService.GetUser(context.User.Id);
        var other = await userService.GetUser(member.Id);

        if (!await userService.TryTransferTo(user, other, amount))
        {
            await context.RespondAsync($"You do not have enough balance to send {amount} credits.\nYour balance: {user.Balance} credits.");
            return;
        }

        var embed = new DiscordEmbedBuilder()
            .WithColor(DiscordColor.White)
            .WithTitle("🤑 You sent money!")
            .WithDescription($"{context.User.Mention} sent {amount} credits to {member.Mention}")
            .AddField($"{context.User.Mention}' Balance", user.Balance.ToString(), true);

        await context.RespondAsync(embed);
    }
}