using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using GrindBot.Application.Services;
using GrindBot.Domain.Common;
using Microsoft.Extensions.Caching.Memory;

namespace GrindBot.DiscordClient.Events;

public sealed class MessageReactionAddedEventHandler(UserService userService, IMemoryCache cache) : IEventHandler<MessageReactionAddedEventArgs>
{
    public async Task HandleEventAsync(DSharpPlus.DiscordClient bot, MessageReactionAddedEventArgs ctx)
    {
        if (ctx.User.IsBot) return;
        if (ctx.Emoji != DiscordEmoji.FromUnicode("⭐")) return;

        var entry = cache.Get<ulong>(CacheKeys.CurrentStarMessageIdKey);
        if (entry != ctx.Message.Id) return;

        cache.Remove(CacheKeys.CurrentStarMessageIdKey);
        await userService.UserStarredMessage(ctx.User.Id);
        await ctx.Message.Channel!.SendMessageAsync($"{ctx.User.Mention} starred a message! ⭐");
    }
}