using System.ComponentModel;
using DSharpPlus.Commands;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Entities;

namespace GrindBot.DiscordClient.Commands;

public sealed class PingCommand
{
    [Command("ping")]
    [Description("Pings the bot")]
    public async ValueTask ExecuteAsync(SlashCommandContext context)
    {
        var embed = new DiscordEmbedBuilder()
            .WithColor(DiscordColor.White)
            .WithAuthor("🏓 Pong!")
            .WithFooter($"Latency: {context.Client.GetConnectionLatency(context.Guild!.Id):g}");

        await context.RespondAsync(embed);
    }
}