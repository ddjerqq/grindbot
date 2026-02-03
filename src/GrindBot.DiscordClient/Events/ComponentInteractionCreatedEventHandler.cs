using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using GrindBot.Application.Services;
using GrindBot.DiscordClient.Commands;

namespace GrindBot.DiscordClient.Events;

public sealed class ComponentInteractionCreatedEventHandler(SamoqalaqoService samoqalaqo) : IEventHandler<ComponentInteractionCreatedEventArgs>
{
    public async Task HandleEventAsync(DSharpPlus.DiscordClient bot, ComponentInteractionCreatedEventArgs ctx)
    {
        switch (ctx.Interaction.Data.CustomId.Split("_"))
        {
            case ["person", "lookup", var first, var last, "one" or "five", "page", var pageString] when int.TryParse(pageString, out var page):
                // we need to delete the previous response and send a new one with updated page
                await ctx.Interaction.CreateResponseAsync(
                    DiscordInteractionResponseType.DeferredChannelMessageWithSource,
                    new DiscordInteractionResponseBuilder().AsEphemeral());
                var people = await samoqalaqo.GetPeopleAsync(first, last);
                var person = people[page];
                var personImage = await samoqalaqo.GetPersonImageAsync(person.Id);
                var personInfoMessage = FindCommand.GetPersonInfoMessage(person, personImage!, page, people.Count);
                await ctx.Interaction.DeleteOriginalResponseAsync();
                await ctx.Interaction.CreateFollowupMessageAsync(
                    new DiscordFollowupMessageBuilder(personInfoMessage).AsEphemeral());
                break;
            case ["clear", var first, var last]:
                await ctx.Interaction.CreateResponseAsync(DiscordInteractionResponseType.UpdateMessage, new DiscordInteractionResponseBuilder().WithContent("Cache cleared!"));
                samoqalaqo.ClearCache(first, last);
                break;
        }
    }
}