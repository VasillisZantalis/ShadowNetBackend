using MediatR;
using ShadowNetBackend.Common;

namespace ShadowNetBackend.Features.Missions.UpdateMission;

public record UpdateMissionCommand(
    Guid Id,
    string? Image,
    MissionStatus Status,
    RiskLevel Risk) : IRequest<bool>;
