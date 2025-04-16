using ShadowNetBackend.Features.Criminals.Common;

namespace ShadowNetBackend.Features.Criminals.GetAllCriminals;

public record GetCriminalsQuery(CriminalParameters Parameters) : IQuery<IEnumerable<CriminalDto>>;

internal class GetCriminalsHandler(
    ApplicationDbContext dbContext,
    ICacheService cache) : IQueryHandler<GetCriminalsQuery, IEnumerable<CriminalDto>>
{
    public async Task<IEnumerable<CriminalDto>> Handle(GetCriminalsQuery request, CancellationToken cancellationToken)
    {
        var cachedCriminals = await cache.GetDataAsync<List<CriminalDto>>(nameof(CacheKeys.Criminals));
        if (cachedCriminals is not null)
            return cachedCriminals;

        var query = dbContext.Criminals.AsQueryable()
            .ApplySorting(request.Parameters.OrderBy)
            .ApplyPagination(request.Parameters.PageSize, request.Parameters.PageNumber);

        if (!string.IsNullOrEmpty(request.Parameters.Name))
            query = query.Where(w => w.FirstName.Contains(request.Parameters.Name) || w.LastName.Contains(request.Parameters.Name));

        if (!string.IsNullOrEmpty(request.Parameters.Alias))
            query = query.Where(w => w.Alias != null && w.Alias.Contains(request.Parameters.Alias));

        var criminals = await query.ToListAsync(cancellationToken);
        var criminalDtos = criminals.ToCriminalDto();

        await cache.SetAsync(nameof(CacheKeys.Criminals), criminalDtos, TimeSpan.FromMinutes(15));

        return criminalDtos;
    }
}
