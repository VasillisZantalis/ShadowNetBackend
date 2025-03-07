using MediatR;
using ShadowNetBackend.Features.Missions.Common;

namespace ShadowNetBackend.Features.Missions.GetByIdMission;

public record GetByIdMissionQuery(Guid Id) : IRequest<MissionResponse>;
