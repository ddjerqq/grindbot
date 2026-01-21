using GrindBot.Domain;
using Microsoft.EntityFrameworkCore;

namespace GrindBot.Application.Abstractions;

public interface IAppDbContext
{
    public DbSet<User> Users { get; }
    public Task<int> SaveChangesAsync(CancellationToken ct = default);
}