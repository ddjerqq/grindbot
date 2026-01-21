using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using GrindBot.Application.Services;
using GrindBot.Domain.Common;
using Microsoft.Extensions.Caching.Memory;

namespace GrindBot.DiscordClient.Events;

public sealed class MessageCreatedEventHandler(UserService userService, IMemoryCache cache) : IEventHandler<MessageCreatedEventArgs>
{
    public async Task HandleEventAsync(DSharpPlus.DiscordClient bot, MessageCreatedEventArgs ctx)
    {
        if (ctx.Author is { IsBot: true }) return;

        if (ctx.Message.Content.Length >= 4)
            await userService.UserSentMessage(ctx.Author.Id);

        // handle star messages
        if (Random.Shared.Next(1, 100) <= 3)
        {
            await ctx.Message.CreateReactionAsync(DiscordEmoji.FromUnicode("⭐"));
            cache.Set(CacheKeys.CurrentStarMessageIdKey, ctx.Message.Id, TimeSpan.FromMinutes(5));
        }
    }
}