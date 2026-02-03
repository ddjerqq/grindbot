using System.ComponentModel;
using DSharpPlus.Commands;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Entities;
using GrindBot.Application.Services;
using Serilog;

namespace GrindBot.DiscordClient.Commands;

public sealed class SherlockCommand(SherlockService sherlock)
{
    [Command("sherlock")]
    [Description("Investigate a username across various social media platforms")]
    public async ValueTask ExecuteAsync(
        SlashCommandContext ctx,
        [Description("The username you want to investigate")] string username,
        [Description("If you want the results to be private and only visible to you")] bool? ephemeral = false)
    {
        var isEphemeral = ephemeral is true;
        await ctx.DeferResponseAsync(isEphemeral);

        Log.Logger.Information("User <{Username} {UserId}> requested an investigation for: {Target}", ctx.User.Username, ctx.User.Id, username);

        var results = await sherlock.Investigate(username);

        if (results.Count == 0)
        {
            await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent($"No results found for username '{username}'."));
            return;
        }

        var embed = new DiscordEmbedBuilder()
            .WithTitle($"Investigation Results for '{username}'")
            .WithColor(DiscordColor.White)
            .WithAuthor("sherlock", "https://github.com/sherlock-project/sherlock", "https://avatars.githubusercontent.com/u/48293496?s=512")
            .WithThumbnail("https://avatars.githubusercontent.com/u/48293496?s=512")
            .WithTimestamp(DateTimeOffset.UtcNow);

        foreach (var result in results)
            embed.AddField(result.Name, result.UrlUser);

        await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed));
    }
}