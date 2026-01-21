using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace GrindBot.Domain;

public sealed class User(ulong id)
{
    public static readonly int XpPerMessage = int.Parse((string)"XP__PER_MESSAGE".FromEnv());
    public static readonly int XpPerStar = int.Parse((string)"XP__PER_STAR".FromEnv());
    public static readonly int DailyRewardAmount = int.Parse((string)"CURRENCY__DAILY_REWARD_AMOUNT".FromEnv());

    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public ulong Id { get; init; } = id;
    public int Xp { get; set; }
    public int Level => Xp / 100;
    
    public int Balance { get; private set; }
    public DateTime DailyCollectedAt { get; private set; } = DateTime.MinValue;

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
    }
}