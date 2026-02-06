using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using GrindBot.Domain.Common;

namespace GrindBot.Domain;

[Table("user_view")]
public sealed record Person
{
    [Column("id")]
    public long Id { get; init; }

    [Column("first_name")]
    public string FirstName { get; init; } = null!;

    [Column("last_name")]
    public string LastName { get; init; } = null!;

    [Column("father_name")]
    public string? FatherName { get; init; }

    [Column("gender")]
    public string? Gender { get; init; }

    [Column("dob")]
    public DateTime DateOfBirth { get; init; }

    [Column("age")]
    public int Age { get; init; }

    [Column("email")]
    public string? Email { get; init; }

    [Column("phone_number")]
    public string? Phone { get; init; }

    [Column("addresses")]
    public string? AddressContainer { get; init; }

    public IEnumerable<string> Addresses => AddressContainer is null
        ? Enumerable.Empty<string>()
        : System.Text.Json.JsonSerializer.Deserialize<List<string>>(AddressContainer.Replace("\\", string.Empty))!;

    [Column("image")]
    public byte[] Image { get; init; } = null!;

    public string CaptionMarkup
    {
        get
        {
            var sb = new StringBuilder();

            sb.AppendLine($"**ID**: {Id}");
            sb.AppendLine($"**Name**: {FirstName.LatinToGeo()} {LastName.LatinToGeo()}");
            sb.AppendLine($"**Age**: {Age:F0} {DateOfBirth:dd/MM/yyyy}");

            if (FatherName is not null)
                sb.AppendLine($"**Father's name**: {FatherName}");

            if (Gender is not null)
                sb.AppendLine($"**Gender**: {(Gender == "M" ? "Male" : "Female")}");

            if (Email is not null)
                sb.AppendLine($"**Email**: {Email}");

            if (Phone is not null)
                sb.AppendLine($"**Phone**: {Phone}");

            foreach (var address in Addresses)
                sb.AppendLine($"**Address**: {address}");

            return sb.ToString();
        }
    }
}