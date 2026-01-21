using System.Diagnostics.CodeAnalysis;
using GrindBot.DiscordClient.Persistence;

namespace GrindBot.DiscordClient.Services;

public sealed class UserService(DiscordDbContext db)
{
    public async Task EnsureUserExistsAsync(ulong id)
    {
        var user = await db.Users.FindAsync(id);
        if (user is not null) return;

        user = new User(id);

        await db.Users.AddAsync(user);
        await db.SaveChangesAsync();
    }

    public async Task UserSentMessage(ulong id)
    {
        var user = await db.Users.FindAsync(id);
        if (user is null) return;

        user.MessageSent();
        await db.SaveChangesAsync();
    }

    public async Task<User?> GetUser(ulong otherId)
    {
        return await db.Users.FindAsync(otherId);
    }

    public async Task UserStarredMessage(ulong userId)
    {
        var user = await db.Users.FindAsync(userId);
        if (user is null) return;

        user.StarCaught();
        await db.SaveChangesAsync();
    }

    public Task<bool> TryCollectDailyReward(User user, [NotNullWhen(false)] out DateTime? collectNextAt)
    {
        return user.TryCollectDaily(out collectNextAt) 
            ? Task.FromResult(false) 
            : db.SaveChangesAsync().ContinueWith(_ => true);
    }

    public async Task<bool> TryTransferTo(User user, User other, int amount)
    {
        if (!user.TryTransfer(other, amount))
            return false;

        await db.SaveChangesAsync();
        return true;
    }
}