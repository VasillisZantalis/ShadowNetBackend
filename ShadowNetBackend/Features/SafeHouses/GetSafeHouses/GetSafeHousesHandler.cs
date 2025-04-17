using ShadowNetBackend.Features.SafeHouses.Common;

namespace ShadowNetBackend.Features.SafeHouses.GetAllSafeHouse;

public record GetSafeHousesQuery(SafeHouseParameters Parameters) : IQuery<IEnumerable<SafeHouseDto>>;

internal class GetSafeHousesHandler(
    ApplicationDbContext dbContext,
    ICacheService cache) : IQueryHandler<GetSafeHousesQuery, IEnumerable<SafeHouseDto>>
{
    public async Task<IEnumerable<SafeHouseDto>> Handle(GetSafeHousesQuery request, CancellationToken cancellationToken)
    {
        var cachedSafeHouses = await cache.GetDataAsync<List<SafeHouseDto>>(nameof(CacheKeys.SafeHouses));
        if (cachedSafeHouses is not null)
            return cachedSafeHouses;

        var query = dbContext.SafeHouses.AsQueryable()
            .ApplySorting(request.Parameters.OrderBy)
            .ApplyPagination(request.Parameters.PageSize, request.Parameters.PageNumber);

        var safeHouses = await query.ToListAsync(cancellationToken);
        var safeHouseDtos = safeHouses.ToSafeHouseDto();

        await cache.SetAsync(nameof(CacheKeys.SafeHouses), safeHouseDtos, TimeSpan.FromHours(2));
        return safeHouseDtos;
    }
}