using System.ComponentModel;
using DSharpPlus.Commands;
using DSharpPlus.Commands.ArgumentModifiers;
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
        [MinMaxValue(0, 1_000_000)] [Description("How much do you want to send")] int amount)
    {
        var user = await userService.GetUser(context.User.Id);
        var other = await userService.GetUser(member.Id);

        if (!user.TryTransfer(other, amount))
        {
            await context.RespondAsync($"You do not have enough balance to send {amount} credits.\nBalance: ${user.Balance}");
            return;
        }

        await context.RespondAsync($"You sent {member.Mention} ${amount}.\nYour Balance: ${user.Balance}\nTheir Balance: ${other.Balance}");
    }
}