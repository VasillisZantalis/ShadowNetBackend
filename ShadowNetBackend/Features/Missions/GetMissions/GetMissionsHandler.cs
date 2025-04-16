using ShadowNetBackend.Features.Missions.Common;

namespace ShadowNetBackend.Features.Missions.GetAllMissions;

public record GetMissionsQuery(MissionParameters Parameters) : IQuery<IEnumerable<MissionDto>>;

internal class GetMissionsHandler(
    ApplicationDbContext dbContext,
    ICacheService cache) : IQueryHandler<GetMissionsQuery, IEnumerable<MissionDto>>
{
    public async Task<IEnumerable<MissionDto>> Handle(GetMissionsQuery request, CancellationToken cancellationToken)
    {
        var cachedMissions = await cache.GetDataAsync<List<MissionDto>>(nameof(CacheKeys.Missions));
        if (cachedMissions is not null)
            return cachedMissions;

        var query = dbContext.Missions.AsQueryable()
            .Include(i => i.AssignedAgents)
            .ApplySorting(request.Parameters.OrderBy)
            .ApplyPagination(request.Parameters.PageSize, request.Parameters.PageNumber);

        if (request.Parameters.Status.HasValue)
            query = query.Where(w => w.Status == request.Parameters.Status.Value);

        if (request.Parameters.Risk.HasValue)
            query = query.Where(w => w.Risk == request.Parameters.Risk.Value);

        var missions = await query.ToListAsync(cancellationToken);

        var missionDtos = missions.Select(mission => new MissionDto
        (
            mission.Id,
            mission.Title,
            mission.Image != null
                ? FileHelper.ConvertToBase64(mission.Image)
                : null,
            mission.Objective,
            mission.Location,
            mission.Date,
            mission.AssignedAgents.ToAgentDto(),
            mission.Status,
            mission.Risk
        ));

        await cache.SetAsync(nameof(CacheKeys.Missions), missionDtos, TimeSpan.FromMinutes(15));

        return missionDtos;
    }
}
