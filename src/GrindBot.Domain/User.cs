using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace GrindBot.Domain;

public sealed class User(ulong id)
{
    public const int XpPerMessage = 1;
    public const int XpPerStar = 25;
    public const int MoneyPerStar = 50;
    public const int DailyRewardAmount = 100;

    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public ulong Id { get; init; } = id;
    public int Xp { get; set; }
    public int Balance { get; private set; }
    public DateTime DailyCollectedAt { get; private set; } = DateTime.MinValue;
    
    
    public int Cookies { get; private set; }
    public DateTime CookieSentAt { get; private set; } = DateTime.MinValue;

    public int Level => Xp / 100;

    public bool TrySendCookie(User receiver, [NotNullWhen(false)] out DateTime? sendNextAt)
    {
        sendNextAt = null;
        
        // check if 24 hours have passed since last collection
        if ((DateTime.UtcNow - CookieSentAt).TotalHours < 24)
        {
            sendNextAt = CookieSentAt.AddHours(24);
            return false;
        }

        receiver.Cookies += 1;
        CookieSentAt = DateTime.UtcNow;
        return true;
    }
    
    public bool TryCollectDaily([NotNullWhen(false)] out DateTime? collectNextAt)
    {
        collectNextAt = null;
        
        // check if 24 hours have passed since last collection
        if ((DateTime.UtcNow - DailyCollectedAt).TotalHours < 24)
        {
            collectNextAt = DailyCollectedAt.AddHours(24);
            return false;
        }
        
        // give daily reward
        Balance += DailyRewardAmount; // daily reward amount
        DailyCollectedAt = DateTime.UtcNow;
        return true;
    }
    
    public bool TryTransfer(User other, int amount)
    {
        if (amount <= 0) return false;
        if (Balance < amount) return false;
        
        Balance -= amount;
        other.Balance += amount;
        return true;
    }
    
    public void MessageSent()
    {
        Xp += XpPerMessage;
    }

    public void StarCaught()
    {
        Xp += XpPerStar;
        Balance += MoneyPerStar;
    }
}