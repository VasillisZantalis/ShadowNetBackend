using MediatR;

namespace ShadowNetBackend.Features.Missions.DeleteMission;

public record DeleteMissionQuery(Guid Id) : IRequest<bool>;
