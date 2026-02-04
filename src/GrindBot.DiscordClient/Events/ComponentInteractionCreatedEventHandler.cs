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
                var people = await samoqalaqo.GetPeopleAsync(first, last);
                var person = people[page];
                var personImage = await samoqalaqo.GetPersonImageAsync(person.Id);
                var personInfoMessage = FindCommand.GetPersonInfoMessage(person, personImage!, page, people.Count);

                await ctx.Interaction.Message!.DeleteAsync();
                await ctx.Interaction.CreateResponseAsync(DiscordInteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder(personInfoMessage));
                break;

            case ["clear", var first, var last]:
                await ctx.Interaction.Message!.DeleteAsync();
                samoqalaqo.ClearCache(first, last);
                await ctx.Interaction.CreateResponseAsync(DiscordInteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("cleared!"));
                await Task.Delay(500);
                await ctx.Interaction.DeleteOriginalResponseAsync();
                break;
        }
    }
}