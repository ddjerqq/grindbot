using System.ComponentModel;
using DSharpPlus.Commands;
using DSharpPlus.Commands.Processors.SlashCommands;
using GrindBot.Application.Services;
using GrindBot.Domain.Common;

namespace GrindBot.DiscordClient.Commands;

public sealed class CookiesCommand(UserService userService)
{
    [Command("cookies")]
    [Description("Gets your or another user's cookie count")]
    public async ValueTask ExecuteAsync( SlashCommandContext context)
    {
        var user = await userService.GetUser(context.User.Id);
        await context.RespondAsync($":cookie: Cookies: {user.Cookies}\nYou can send a cookie in {user.CookieSentAt.AddDays(1).FormatRelative()}.");
    }
}

