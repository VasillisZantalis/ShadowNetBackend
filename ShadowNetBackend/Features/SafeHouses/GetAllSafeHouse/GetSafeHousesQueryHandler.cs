using MediatR;
using Microsoft.EntityFrameworkCore;
using ShadowNetBackend.Common;
using ShadowNetBackend.Extensions;
using ShadowNetBackend.Features.SafeHouses.Common;
using ShadowNetBackend.Infrastructure.Data;
using ShadowNetBackend.Infrastructure.Interfaces;
using ShadowNetBackend.Mappings;

namespace ShadowNetBackend.Features.SafeHouses.GetAllSafeHouse;

public class GetSafeHousesQueryHandler : IRequestHandler<GetSafeHousesQuery, IEnumerable<SafeHouseResponse>>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ICacheService _cache;

    public GetSafeHousesQueryHandler(ApplicationDbContext dbContext, ICacheService cache)
    {
        _dbContext = dbContext;
        _cache = cache;
    }

    public async Task<IEnumerable<SafeHouseResponse>> Handle(GetSafeHousesQuery request, CancellationToken cancellationToken)
    {
        var cachedSafeHouses = await _cache.GetDataAsync<List<SafeHouseResponse>>(nameof(CacheKeys.SafeHouses));
        if (cachedSafeHouses is not null)
        {
            return cachedSafeHouses;

        }
        var query = _dbContext.SafeHouses.AsQueryable()
            .ApplySorting(request.Parameters.OrderBy)
            .ApplyPagination(request.Parameters.PageSize, request.Parameters.PageNumber);

        var safeHouses = await query.ToListAsync(cancellationToken);
        var safeHouseResponses = safeHouses.Select(s => s.ToSafeHouseResponse()).ToList();

        await _cache.SetAsync(nameof(CacheKeys.SafeHouses), safeHouseResponses, TimeSpan.FromHours(2));

        return safeHouseResponses;
    }
}
