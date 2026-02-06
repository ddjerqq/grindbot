using System.ComponentModel;
using DSharpPlus.Commands;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Entities;
using GrindBot.Application.Services;
using GrindBot.Domain.Common;

namespace GrindBot.DiscordClient.Commands;

public sealed class SendCookieCommand(UserService userService)
{
    [Command("send_cookie")]
    [Description("Send another user a cookie (24 hour cooldown")]
    public async ValueTask ExecuteAsync(
        SlashCommandContext context,
        [Description("The user who you want to give the cookie to")] DiscordUser member)
    {
        var user = await userService.GetUser(context.User.Id);
        var other = await userService.GetUser(member.Id);

        if (!user.TrySendCookie(other, out var sendNextAt))
        {
            await context.RespondAsync($"❌ You need to wait! Next cookie can be sent at {sendNextAt.FormatRelative()}.");
            return;
        }

        await context.RespondAsync($":cookie: {member.Mention}! You got a cookie from {context.User.Mention}! nom nom nom");
    }
}