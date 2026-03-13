using System.ComponentModel;
using System.Text.RegularExpressions;
using DSharpPlus.Commands;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Entities;
using GrindBot.Application.Services;
using Serilog;
using Microsoft.Extensions.Caching.Memory;

namespace GrindBot.DiscordClient.Commands;

public sealed partial class SherlockCommand(SherlockService sherlock)
{
    private const int PageSize = 10;

    [Command("sherlock")]
    [Description("Investigate a username across various social media platforms")]
    public async ValueTask ExecuteAsync(
        SlashCommandContext ctx,
        [Description("The username you want to investigate")] string username,
        [Description("If you want the results to be private and only visible to you")] bool? ephemeral = false)
    {
        var isEphemeral = ephemeral is true;
        await ctx.DeferResponseAsync(isEphemeral);

        Log.Logger.Information("User {Username} <@{UserId}> requested an investigation for: {Target}", ctx.User.Username, ctx.User.Id, username);

        if (!UsernameMatch().IsMatch(username))
        {
            Log.Logger.Warning("User {Username} <@{UserId}> requested an investigation for an invalid target: {Target}", ctx.User.Username, ctx.User.Id, username);
            await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("Invalid username. Usernames can only contain letters, numbers, and underscores."));
            return;
        }

        var results = await sherlock.Investigate(username);

        if (results.Count == 0)
        {
            await ctx.EditResponseAsync(
                new DiscordWebhookBuilder()
                    .WithContent($"No results found for username '{username}'."));
            return;
        }


        var builder = BuildPage(username, results, 0);

        await ctx.EditResponseAsync(builder);
    }

    public static DiscordWebhookBuilder BuildPage(string username, List<SherlockResponse> results, int page)
    {
        var totalPages = (int)Math.Ceiling(results.Count / (double)PageSize);

        var embed = new DiscordEmbedBuilder()
            .WithTitle($"Investigation Results for '{username}'")
            .WithColor(DiscordColor.White)
            .WithAuthor("sherlock", "https://github.com/sherlock-project/sherlock", "https://avatars.githubusercontent.com/u/48293496?s=512")
            .WithThumbnail("https://avatars.githubusercontent.com/u/48293496?s=512")
            .WithFooter($"Page {page + 1}/{totalPages}")
            .WithTimestamp(DateTimeOffset.UtcNow);

        foreach (var result in results
            .Take(PageSize))
        {
            embed.AddField(result.Name, result.UrlUser);
        }

        var builder = new DiscordWebhookBuilder().AddEmbed(embed);
        var previousDisabled = page == 0;
        var nextDisabled = page == totalPages - 1;

        builder.AddActionRowComponent(
            new DiscordButtonComponent(
                DiscordButtonStyle.Secondary,
                $"sherlock_prev_{username}_{page}",
                "Prev",
                previousDisabled),

            new DiscordButtonComponent(
                DiscordButtonStyle.Secondary,
                $"sherlock_next_{username}_{page}",
                "Next",
                nextDisabled)
        );

        return builder;
    }

    [GeneratedRegex("^[a-zA-Z0-9_]+$")]
    private static partial Regex UsernameMatch();
}