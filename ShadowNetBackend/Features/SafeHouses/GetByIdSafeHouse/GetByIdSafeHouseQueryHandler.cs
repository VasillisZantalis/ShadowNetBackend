using ShadowNetBackend.Features.SafeHouses.Common;

namespace ShadowNetBackend.Features.SafeHouses.GetByIdSafeHouse;

public class GetByIdSafeHouseQueryHandler : IRequestHandler<GetByIdSafeHouseQuery, SafeHouseResponse>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ICacheService _cache;

    public GetByIdSafeHouseQueryHandler(ApplicationDbContext dbContext, ICacheService cache)
    {
        _dbContext = dbContext;
        _cache = cache;
    }

    public async Task<SafeHouseResponse> Handle(GetByIdSafeHouseQuery request, CancellationToken cancellationToken)
    {
        string cacheKey = $"{CacheKeys.Agents}_{request.Id}";

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
        await _cache.SetAsync(cacheKey, safeHouseResponse, TimeSpan.FromHours(2));

        return safeHouseResponse;
    }
}
