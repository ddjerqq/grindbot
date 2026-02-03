using System.ComponentModel.DataAnnotations.Schema;
using GrindBot.Domain.Common;

namespace GrindBot.Domain;

[Table("user")]
public sealed record Person
{
    [Column("id")]
    public long Id { get; init; }

    [Column("first_name")]
    public string FirstName { get; init; } = null!;

    [Column("last_name")]
    public string LastName { get; init; } = null!;

    [Column("dob")]
    public DateTime DateOfBirth { get; init; }

    public float Age => (DateTime.Now - DateOfBirth).Days / 365f;

    [Column("legal_address")]
    public string LegalAddress { get; init; } = null!;

    [Column("actual_address")]
    public string? ActualAddress { get; init; } = null!;

    public string Address => string.IsNullOrWhiteSpace(ActualAddress)
        ? LegalAddress
        : $"Legal: {LegalAddress}\nActual: {ActualAddress}";

    public string CaptionMarkup =>
        $"""
         ID: {Id}
         Name: {FirstName.LatinToGeo()} {LastName.LatinToGeo()}
         Age: {Age:F1} {DateOfBirth:dd/MM/yyyy}
         Address: {Address.LatinToGeo()}
         """;
}