namespace ShadowNetBackend.Dtos.Criminals;

public class CriminalForCreationDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Alias { get; set; }
    public DateTimeOffset? DateOfBirth { get; set; }
    public string? Nationality { get; set; }
    public string? KnownAffiliations { get; set; }
    public RiskLevel ThreatLevel { get; set; } = RiskLevel.Low;
    public bool IsArmedAndDangerous { get; set; } = false;
    public string? LastKnownLocation { get; set; }
    public DateTime? LastSpottedDate { get; set; }
    public bool UnderSurveillance { get; set; } = false;
    public string? SurveillanceNotes { get; set; }
    public string? Image { get; set; }
}
