using GrindBot.Application.Abstractions;
using GrindBot.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace GrindBot.Application.Services;

public sealed class SamoqalaqoService(ISamoqalaqoRepository db, IMemoryCache cache)
{
    public async Task<List<Person>> GetPeopleAsync(string firstName, string lastName, CancellationToken ct = default)
    {
        var cacheKey = $"person_lookup_{firstName}_{lastName}";
        return await cache.GetOrCreateAsync(cacheKey, async entry =>
        {
            entry.SetSlidingExpiration(TimeSpan.FromMinutes(5));

            var people = await db.People
                .Where(x => x.FirstName == firstName && x.LastName == lastName)
                .OrderByDescending(x => x.DateOfBirth)
                .ToListAsync(ct);

            return people;
        }) ?? [];
    }

    public void ClearCache(string firstName, string lastName)
    {
        var cacheKey = $"person_lookup_{firstName}_{lastName}";
        cache.Remove(cacheKey);
    }
}