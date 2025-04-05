namespace ShadowNetBackend.Features.Criminals;

public class Criminal
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Alias { get; set; }
    public DateTimeOffset? DateOfBirth { get; set; }
    public string? Nationality { get; set; }
    public string? KnownAffiliations { get; set; }
    public RiskLevel ThreatLevel { get; set; }
    public bool IsArmedAndDangerous { get; set; }
    public string? LastKnownLocation { get; set; }
    public DateTime? LastSpottedDate { get; set; }
    public bool UnderSurveillance { get; set; }
    public string? SurveillanceNotes { get; set; }
    public byte[]? Image { get; set; }
}
