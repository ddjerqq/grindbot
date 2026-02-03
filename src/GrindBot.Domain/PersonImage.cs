using System.ComponentModel.DataAnnotations.Schema;

namespace GrindBot.Domain;

[Table("user_image")]
public sealed record PersonImage
{
    [Column("id")]
    public long Id { get; init; }

    [Column("image")]
    public byte[] Image { get; init; } = null!;
}