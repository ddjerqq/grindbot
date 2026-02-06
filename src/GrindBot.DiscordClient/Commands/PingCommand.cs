using System.ComponentModel;
using DSharpPlus.Commands;
using DSharpPlus.Commands.Processors.SlashCommands;

namespace GrindBot.DiscordClient.Commands;

public sealed class PingCommand
{
    [Command("ping")]
    [Description("Pings the bot")]
    public async ValueTask ExecuteAsync(SlashCommandContext context)
    {
        await context.RespondAsync($"🏓 Pong!\n-# Latency: {context.Client.GetConnectionLatency(context.Guild!.Id).Milliseconds}ms");
    }
}