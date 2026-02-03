using GrindBot.Domain.Common;

namespace GrindBot.Application.Services;

public sealed class WhiteListService
{
    private static readonly string WhitelistFilePath = "TELEGRAM_WHITELIST_PATH".FromEnv();
    private readonly HashSet<long> _whitelistedUserIds;

    public WhiteListService()
    {
        if (!File.Exists(WhitelistFilePath)) throw new InvalidOperationException($"Whitelist file not found at path: {WhitelistFilePath}");
        var json = File.ReadAllText(WhitelistFilePath);
        var list = System.Text.Json.JsonSerializer.Deserialize<List<long>>(json);
        _whitelistedUserIds = list != null ? [..list] : [];
    }
    
    public IEnumerable<long> WhitelistedUserIds => _whitelistedUserIds.AsReadOnly();
    public bool IsWhiteListed(long? userId) => userId is not null && _whitelistedUserIds.Contains(userId.Value);
    public async Task AddToWhiteListAsync(long userId, CancellationToken ct = default)
    {
        if (!_whitelistedUserIds.Add(userId)) return;
        var json = System.Text.Json.JsonSerializer.Serialize(_whitelistedUserIds.ToList());
        await File.WriteAllTextAsync(WhitelistFilePath, json, ct);
    }

    public async Task RemoveFromWhiteListAsync(long userId, CancellationToken ct = default)
    {
        if (!_whitelistedUserIds.Remove(userId)) return;
        var json = System.Text.Json.JsonSerializer.Serialize(_whitelistedUserIds.ToList());
        await File.WriteAllTextAsync(WhitelistFilePath, json, ct);
    }
}