using MediatR;

namespace ShadowNetBackend.Features.Missions.DeleteMission;

public record DeleteMissionCommand(Guid Id) : IRequest<bool>;
