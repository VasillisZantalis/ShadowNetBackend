namespace ShadowNetBackend.Dtos.Criminals;

public record CriminalForUpdateDto(
    Guid Id,
    string FirstName,
    string LastName,
    string? Alias,
    DateTimeOffset? DateOfBirth,
    string? Nationality,
    string? KnownAffiliations,
    RiskLevel ThreatLevel,
    bool IsArmedAndDangerous,
    string? LastKnownLocation,
    DateTime? LastSpottedDate,
    bool UnderSurveillance,
    string? SurveillanceNotes,
    string? Image);
