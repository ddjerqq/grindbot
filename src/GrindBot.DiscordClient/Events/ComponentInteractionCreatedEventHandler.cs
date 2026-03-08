using DSharpPlus;
using DSharpPlus.EventArgs;

namespace GrindBot.DiscordClient.Events;

public sealed class ComponentInteractionCreatedEventHandler : IEventHandler<ComponentInteractionCreatedEventArgs>
{
    public async Task HandleEventAsync(DSharpPlus.DiscordClient bot, ComponentInteractionCreatedEventArgs ctx)
    {
        switch (ctx.Interaction.Data.CustomId.Split("_"))
        {
            // case ["person", "lookup", var first, var last, "one" or "five", "page", var pageString] when int.TryParse(pageString, out var page):
            //     var people = await samoqalaqo.GetPeopleAsync(first, last);
            //     var person = people[page];
            //     var personInfoMessage = FindCommand.GetPersonInfoMessage(person, page, people.Count);
            //     await ctx.Interaction.CreateResponseAsync(DiscordInteractionResponseType.UpdateMessage, new DiscordInteractionResponseBuilder(personInfoMessage).AsEphemeral());
            //     break;
            //
            // case ["clear", var first, var last]:
            //     samoqalaqo.ClearCache(first, last);
            //     await ctx.Interaction.CreateResponseAsync(DiscordInteractionResponseType.UpdateMessage, new DiscordInteractionResponseBuilder().WithContent("Cleared!").AsEphemeral());
            //     await ctx.Interaction.DeleteOriginalResponseAsync();
            //     break;
        }
    }
}