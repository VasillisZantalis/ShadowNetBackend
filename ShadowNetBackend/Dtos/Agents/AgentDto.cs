namespace ShadowNetBackend.Dtos.Agents;

public record AgentDto(
    Guid Id,
    string FirstName,
    string LastName,
    string? Image = null,
    string? Alias = null,
    UserRank Rank = UserRank.Agent,
    string? Specialization = null,
    ClearanceLevel ClearanceLevel = ClearanceLevel.None
);