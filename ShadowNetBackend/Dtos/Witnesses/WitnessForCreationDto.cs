namespace ShadowNetBackend.Dtos.Witnesses;

public record WitnessForCreationDto(
    string FirstName,
    string LastName,
    string Alias,
    string? Image,
    RiskLevel RiskLevel,
    string? LocationHistory,
    RelocationStatus RelocationStatus);