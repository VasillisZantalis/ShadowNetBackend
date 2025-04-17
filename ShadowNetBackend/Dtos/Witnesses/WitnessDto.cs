namespace ShadowNetBackend.Dtos.Witnesses;

public record WitnessDto(
    Guid Id,
    string FirstName,
    string LastName,
    string Alias,
    RiskLevel RiskLevel,
    string? LocationHistory,
    RelocationStatus RelocationStatus);