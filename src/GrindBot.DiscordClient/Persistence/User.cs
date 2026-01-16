using System.ComponentModel.DataAnnotations.Schema;
using GrindBot.DiscordClient.Common;

namespace GrindBot.DiscordClient.Persistence;

public sealed class User
{
    public static readonly int XpPerMessage = int.Parse("XP__PER_MESSAGE".FromEnv());
    public static readonly int XpPerStar = int.Parse("XP__PER_STAR".FromEnv());

    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public ulong Id { get; init; }

    public int Xp { get; set; }

    public User(ulong id)
    {
        Id = id;
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