using GrindBot.Application.Abstractions;
using GrindBot.Domain;
using Microsoft.EntityFrameworkCore;

namespace GrindBot.Infrastructure.Persistence;

public sealed class SamoqalaqoDbContext(DbContextOptions<SamoqalaqoDbContext> options) : DbContext(options), ISamoqalaqoRepository
{
    public DbSet<Person> People => Set<Person>();
}
