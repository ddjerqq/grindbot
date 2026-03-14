using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using GrindBot.Application.Services;
using GrindBot.DiscordClient.Commands;
using Microsoft.Extensions.Caching.Memory;

namespace GrindBot.DiscordClient.Events;

public sealed class ComponentInteractionCreatedEventHandler(IMemoryCache cache) : IEventHandler<ComponentInteractionCreatedEventArgs>
{
    public async Task HandleEventAsync(DSharpPlus.DiscordClient bot, ComponentInteractionCreatedEventArgs ctx)
    {
        switch (ctx.Interaction.Data.CustomId.Split("_"))
        {
            case ["sherlock", ("prev" or "next") and var action, var username, var pageString] when int.TryParse(pageString, out var page):
                await HandleSherlock(ctx, action, username, page);
                break;
        }
    }

    private async Task HandleSherlock(ComponentInteractionCreatedEventArgs ctx, string action, string username, int page)
    {
        var cachedResult = cache.Get<List<SherlockResponse>>(username) ?? throw new InvalidOperationException("Cache miss");
        page += action == "next" ? 1 : -1;
        var webhook = SherlockCommand.BuildPage(username, cachedResult, page);

        var response = new DiscordInteractionResponseBuilder();

        foreach (var embed in webhook.Embeds)
            response.AddEmbed(embed);

        foreach (var row in webhook.ComponentActionRows ?? [])
            response.AddActionRowComponent(row);

        await ctx.Interaction.CreateResponseAsync(DiscordInteractionResponseType.UpdateMessage, response);
    }
}