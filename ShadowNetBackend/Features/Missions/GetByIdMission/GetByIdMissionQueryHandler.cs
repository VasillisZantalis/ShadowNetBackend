using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ShadowNetBackend.Common;
using ShadowNetBackend.Features.Agents.Common;
using ShadowNetBackend.Features.Missions.Common;
using ShadowNetBackend.Helpers;
using ShadowNetBackend.Infrastructure.Data;
using ShadowNetBackend.Infrastructure.Interfaces;
using ShadowNetBackend.Mappings;

namespace ShadowNetBackend.Features.Missions.GetByIdMission;

public class GetByIdMissionQueryHandler : IRequestHandler<GetByIdMissionQuery, MissionResponse>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ICryptographyService _cryptographyService;
    private readonly ICacheService _cache;
    private readonly RedisCacheSettings _cacheSettings;

    public GetByIdMissionQueryHandler(ApplicationDbContext dbContext, ICryptographyService cryptographyService, ICacheService cache, IOptions<RedisCacheSettings> cacheSettings)
    {
        _dbContext = dbContext;
        _cryptographyService = cryptographyService;
        _cache = cache;
        _cacheSettings = cacheSettings.Value;
    }

    public async Task<MissionResponse> Handle(GetByIdMissionQuery request, CancellationToken cancellationToken)
    {
        string cacheKey = $"{CacheKeys.Mission}_{request.Id}";

        var cachedMission = await _cache.GetDataAsync<MissionResponse>(cacheKey);
        if (cachedMission is not null)
        {
            return cachedMission;
        }

        var mission = await _dbContext.Missions
            .AsNoTracking()
            .Include(x => x.AssignedAgents)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (mission == null)
            throw new MissionNotFoundException();

        var encryptionType = request.EncryptionType ?? EncryptionType.None;

        var missionResponse =  new MissionResponse
        {
            Id = mission.Id,
            Title = mission.Title,
            Image = mission.Image != null
                ? FileHelper.ConvertToBase64(mission.Image)
                : null,
            Objective = encryptionType != EncryptionType.None
                ? _cryptographyService.Decrypt(mission.Objective, encryptionType, request.EncryptionKey)
                : mission.Objective,
            Location = encryptionType != EncryptionType.None
                ? _cryptographyService.Decrypt(mission.Location, encryptionType, request.EncryptionKey)
                : mission.Location,
            Status = mission.Status,
            Risk = mission.Risk,
            Date = mission.Date,
            AssignedAgents = mission.AssignedAgents.Select(x => x.ToAgentResponse()).ToList()
        };

        await _cache.SetAsync(cacheKey, missionResponse, TimeSpan.FromSeconds(_cacheSettings.DefaultSlidingExpiration));

        return missionResponse;
    }
}
