using System.ComponentModel;
using System.Text.RegularExpressions;
using DSharpPlus.Commands;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Entities;
using GrindBot.Application.Services;
using GrindBot.Domain;
using Serilog;

namespace GrindBot.DiscordClient.Commands;

public sealed partial class FindCommand(SamoqalaqoService samoqalaqo)
{
    [Command("find")]
    [Description("Find a person in the database by their first and last name (supports latin and georgian letters)")]
    public async ValueTask ExecuteAsync(SlashCommandContext context, [Description("first name")] string firstName, [Description("last name")] string lastName)
    {
        await context.DeferResponseAsync(ephemeral: true);

        var nameMatch = NameMatch();
        if (!nameMatch.IsMatch(firstName) || !nameMatch.IsMatch(lastName))
        {
            Log.Logger.Warning("User {Username} <@{UserId}> requested invalid lookup for {First} {Last}", context.User.Username, context.User.Id, firstName, lastName);
            await context.FollowupAsync("First and lastName name must be provided.");
            return;
        }

        var people = await samoqalaqo.GetPeopleAsync(firstName, lastName);

        Log.Logger.Information("User {Username} <@{UserId}> requested lookup for {First} {Last}, found {Count} results", context.User.Username, context.User.Id, firstName, lastName, people.Count);

        if (people.Count == 0)
        {
            await context.FollowupAsync("404 - Not found");
        }
        else
        {
            var person = people.First();
            var personInfoMessage = GetPersonInfoMessage(person, 0, people.Count);
            await context.FollowupAsync(new DiscordFollowupMessageBuilder(personInfoMessage).AsEphemeral());
        }
    }

    public static DiscordMessageBuilder GetPersonInfoMessage(Person person, int page, int total)
    {
        var messageBuilder = total > 1
            ? GetPaginationKeyboard(person, page, total)
            : new DiscordMessageBuilder();

        var embed = new DiscordEmbedBuilder()
            .WithColor(DiscordColor.White)
            .WithDescription(person.CaptionMarkup)
            .WithImageUrl($"attachment://{person.Id}.png")
            .WithTimestamp(DateTimeOffset.UtcNow.AddMinutes(5))
            .WithFooter("This message will self-destruct")
            .Build();

        messageBuilder.AddEmbed(embed);

        using var stream = new MemoryStream(person.Image);
        messageBuilder.AddFile($"{person.Id}.png", stream, AddFileOptions.CopyStream);

        return messageBuilder;
    }

    private static DiscordMessageBuilder GetPaginationKeyboard(Person person, int page, int total)
    {
        return new DiscordMessageBuilder()
            .AddActionRowComponent(
                new DiscordButtonComponent(DiscordButtonStyle.Secondary, $"person_lookup_{person.FirstName}_{person.LastName}_one_page_{Math.Max(0, page - 1)}", "◀️"),
                new DiscordButtonComponent(DiscordButtonStyle.Danger, $"clear_{person.FirstName}_{person.LastName}", "❌"),
                new DiscordButtonComponent(DiscordButtonStyle.Secondary, $"person_lookup_{person.FirstName}_{person.LastName}_one_page_{Math.Min(total, page + 1)}", "▶️"))
            .AddActionRowComponent(
                new DiscordButtonComponent(DiscordButtonStyle.Secondary, $"person_lookup_{person.FirstName}_{person.LastName}_five_page_{Math.Max(0, page - 5)}", "⏮️"),
                new DiscordButtonComponent(DiscordButtonStyle.Success, "page_number", total > 1 ? $"{page + 1} / {total}" : "1/1", true),
                new DiscordButtonComponent(DiscordButtonStyle.Secondary, $"person_lookup_{person.FirstName}_{person.LastName}_five_page_{Math.Min(total, page + 5)}", "⏭️"));
    }

    [GeneratedRegex("[ა-ზa-zA-Z]")]
    private static partial Regex NameMatch();
}