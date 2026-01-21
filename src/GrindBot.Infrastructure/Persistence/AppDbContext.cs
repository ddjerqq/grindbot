using GrindBot.Application.Abstractions;
using GrindBot.Domain;
using Microsoft.EntityFrameworkCore;

namespace GrindBot.Infrastructure.Persistence;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options), IAppDbContext
{
    public DbSet<User> Users => Set<User>();
}