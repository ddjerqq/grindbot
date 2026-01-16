using Microsoft.EntityFrameworkCore;

namespace GrindBot.DiscordClient.Persistence;

public sealed class DiscordDbContext(DbContextOptions<DiscordDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
}