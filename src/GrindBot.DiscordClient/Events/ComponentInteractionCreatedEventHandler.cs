using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using GrindBot.DiscordClient.Commands;

namespace GrindBot.DiscordClient.Events;

public sealed class ComponentInteractionCreatedEventHandler : IEventHandler<ComponentInteractionCreatedEventArgs>
{
    public async Task HandleEventAsync(DSharpPlus.DiscordClient bot, ComponentInteractionCreatedEventArgs ctx)
    {
        var parts = ctx.Interaction.Data.CustomId.Split("_");

        if (parts.Length == 0)
            return;

        var command = parts[0];

        switch (command)
        {
            case "sherlock":
                await HandleSherlock(ctx, parts);
                break;

            case "anothercommand":
                await HandleAnotherCommand(ctx, parts);
                break;
        }
    }

    private static async Task HandleSherlock(ComponentInteractionCreatedEventArgs ctx, string[] parts)
    {
        if (parts.Length != 4)
            return;

        var action = parts[1];
        var username = parts[2];

        if (!int.TryParse(parts[3], out var page))
            return;

        if (!SherlockCommand.TryGetCache(username, out var results))
            return;

        page = action switch
        {
            "next" => page + 1,
            "prev" => page - 1,
            _ => page
        };

        var webhook = SherlockCommand.BuildPage(username, results, page);

        var response = new DiscordInteractionResponseBuilder();

        foreach (var embed in webhook.Embeds)
            response.AddEmbed(embed);

        foreach (var row in webhook.ComponentActionRows)
            response.AddActionRowComponent(row);

        await ctx.Interaction.CreateResponseAsync(
            DiscordInteractionResponseType.UpdateMessage,
            response);
    }

    private static async Task HandleAnotherCommand(ComponentInteractionCreatedEventArgs ctx, string[] parts)
    {
        // Copy for another command
    }
}