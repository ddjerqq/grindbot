using GrindBot.Domain;
using Microsoft.EntityFrameworkCore;

namespace GrindBot.Application.Abstractions;

public interface ISamoqalaqoRepository
{
    public DbSet<Person> People { get; }
    public DbSet<PersonImage> PersonImages { get; }
}