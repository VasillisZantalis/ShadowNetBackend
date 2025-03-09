using ShadowNetBackend.Common;

namespace ShadowNetBackend.Features.Witnesses.Common;

public record WitnessResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string Alias,
    RiskLevel RiskLevel,
    string? LocationHistory,
    RelocationStatus RelocationStatus);
