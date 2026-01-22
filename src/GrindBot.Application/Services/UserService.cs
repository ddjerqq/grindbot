using System.Diagnostics.CodeAnalysis;
using GrindBot.Application.Abstractions;
using GrindBot.Domain;

namespace GrindBot.Application.Services;

public sealed class UserService(IAppDbContext db) : IAsyncDisposable
{
    /// <summary>
    /// Ensures the user with the provided snowflake id exists in the database.
    /// </summary>
    /// <returns>True if exists, false otherwise</returns>
    public async Task<bool> EnsureUserExistsAsync(ulong id)
    {
        var user = await db.Users.FindAsync(id);
        if (user is not null) return true;

        user = new User(id);

        await db.Users.AddAsync(user);
        await db.SaveChangesAsync();
        
        return false;
    }

    public async Task<User> GetUser(ulong otherId)
    {
        return await db.Users.FindAsync(otherId) ?? throw new InvalidOperationException("User not found");
    }

    public async ValueTask DisposeAsync()
    {
        await db.SaveChangesAsync();
    }
}