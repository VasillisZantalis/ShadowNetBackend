using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ShadowNetBackend.Common;
using ShadowNetBackend.Features.Agents.Common;
using ShadowNetBackend.Features.SafeHouses.Common;
using ShadowNetBackend.Infrastructure.Data;
using ShadowNetBackend.Infrastructure.Interfaces;
using ShadowNetBackend.Mappings;

namespace ShadowNetBackend.Features.SafeHouses.GetByIdSafeHouse;

public class GetByIdSafeHouseQueryHandler : IRequestHandler<GetByIdSafeHouseQuery, SafeHouseResponse>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly RedisCacheSettings _cacheSettings;
    private readonly ICacheService _cache;

    public GetByIdSafeHouseQueryHandler(ApplicationDbContext dbContext, ICacheService cache, IOptions<RedisCacheSettings> cacheSettings)
    {
        _dbContext = dbContext;
        _cache = cache;
        _cacheSettings = cacheSettings.Value;
    }

    public async Task<SafeHouseResponse> Handle(GetByIdSafeHouseQuery request, CancellationToken cancellationToken)
    {
        string cacheKey = $"{CacheKeys.Agent}_{request.Id}";

        var cachedSafeHouse = await _cache.GetDataAsync<SafeHouseResponse>(cacheKey);
        if (cachedSafeHouse is not null)
        {
            return cachedSafeHouse;
        }

        var safeHouse = await _dbContext.SafeHouses
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (safeHouse == null)
            throw new SafeHouseNotFoundException();


        var safeHouseResponse = safeHouse.ToSafeHouseResponse();
        await _cache.SetAsync(cacheKey, safeHouseResponse, TimeSpan.FromSeconds(_cacheSettings.DefaultSlidingExpiration));

        return safeHouseResponse;
    }
}
