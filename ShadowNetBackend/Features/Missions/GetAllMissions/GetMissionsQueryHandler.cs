using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ShadowNetBackend.Common;
using ShadowNetBackend.Extensions;
using ShadowNetBackend.Features.Agents.Common;
using ShadowNetBackend.Features.Missions.Common;
using ShadowNetBackend.Helpers;
using ShadowNetBackend.Infrastructure.Data;
using ShadowNetBackend.Infrastructure.Interfaces;

namespace ShadowNetBackend.Features.Missions.GetAllMissions;

public class GetMissionsQueryHandler : IRequestHandler<GetMissionsQuery, IEnumerable<MissionResponse>>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ICacheService _cache;
    private readonly RedisCacheSettings _cacheSettings;

    public GetMissionsQueryHandler(ApplicationDbContext dbContext, ICacheService cache, IOptions<RedisCacheSettings> cacheSettings)
    {
        _dbContext = dbContext;
        _cache = cache;
        _cacheSettings = cacheSettings.Value;
    }

    public async Task<IEnumerable<MissionResponse>> Handle(GetMissionsQuery request, CancellationToken cancellationToken)
    {
        var cachedMissions = await _cache.GetDataAsync<List<MissionResponse>>(nameof(CacheKeys.Missions));
        if (cachedMissions is not null)
        {
            return cachedMissions;
        }

        var query = _dbContext.Missions.AsQueryable()
            .ApplySorting(request.Parameters.OrderBy)
            .ApplyPagination(request.Parameters.PageSize, request.Parameters.PageNumber);

        if (request.Parameters.Status.HasValue)
        {
            query = query.Where(w => w.Status == request.Parameters.Status.Value);
        }

        if (request.Parameters.Risk.HasValue)
        {
            query = query.Where(w => w.Risk == request.Parameters.Risk.Value);
        }

        var missions = await query.ToListAsync(cancellationToken);

        var missionResponses = missions.Select(mission => new MissionResponse
        {
            Id = mission.Id,
            Title = mission.Title,
            Image = mission.Image != null
                ? FileHelper.ConvertToBase64(mission.Image)
                : null,
            Objective = mission.Objective,
            Location = mission.Location,
            Status = mission.Status,
            Risk = mission.Risk,
            Date = mission.Date
        }).ToList();

        await _cache.SetAsync(nameof(CacheKeys.Missions), missionResponses, TimeSpan.FromSeconds(_cacheSettings.DefaultSlidingExpiration));

        return missionResponses;
    }
}
