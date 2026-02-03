using GrindBot.Application.Abstractions;
using GrindBot.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace GrindBot.Application.Services;

public sealed class SamoqalaqoService(ISamoqalaqoRepository db, IMemoryCache cache)
{
    public async Task<byte[]?> GetPersonImageAsync(long id, bool thumbnail = false, CancellationToken ct = default)
    {
        var cacheKey = $"person_image_{id}";
        var personImage = await cache.GetOrCreateAsync(cacheKey, async entry =>
        {
            entry.SetSlidingExpiration(TimeSpan.FromHours(1));

            return await db.PersonImages
                .Where(x => x.Id == id)
                .SingleOrDefaultAsync(ct);
        });

        if (personImage is null) return null;

        if (thumbnail)
        {
            await using var inputStream = new MemoryStream(personImage.Image);
            using var image = await Image.LoadAsync(inputStream, ct);
            await using var outputStream = new MemoryStream();
            var encoder = new JpegEncoder { Quality = 10 };
            await image.SaveAsJpegAsync(outputStream, encoder, ct);
            return outputStream.ToArray();
        }

        return personImage.Image;
    }

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