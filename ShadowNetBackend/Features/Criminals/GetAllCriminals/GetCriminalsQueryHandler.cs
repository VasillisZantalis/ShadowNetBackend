using ShadowNetBackend.Features.Criminals.Common;

namespace ShadowNetBackend.Features.Criminals.GetAllCriminals;

public class GetCriminalsQueryHandler : IRequestHandler<GetCriminalsQuery, IEnumerable<CriminalResponse>>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ICacheService _cache;

    public GetCriminalsQueryHandler(ApplicationDbContext dbContext, ICacheService cache)
    {
        _dbContext = dbContext;
        _cache = cache;
    }

    public async Task<IEnumerable<CriminalResponse>> Handle(GetCriminalsQuery request, CancellationToken cancellationToken)
    {
        var cachedCriminals = await _cache.GetDataAsync<List<CriminalResponse>>(nameof(CacheKeys.Criminals));
        if (cachedCriminals is not null)
        {
            return cachedCriminals;
        }

        var query = _dbContext.Criminals.AsQueryable()
            .ApplySorting(request.Parameters.OrderBy)
            .ApplyPagination(request.Parameters.PageSize, request.Parameters.PageNumber);

        if (!string.IsNullOrEmpty(request.Parameters.Name))
            query = query.Where(w => w.FirstName.Contains(request.Parameters.Name) || w.LastName.Contains(request.Parameters.Name));

        if (!string.IsNullOrEmpty(request.Parameters.Alias))
            query = query.Where(w => w.Alias != null && w.Alias.Contains(request.Parameters.Alias));

        var criminals = await query.ToListAsync(cancellationToken);
        var criminalResponses = criminals.Select(s => s.ToCriminalResponse()).ToList();

        await _cache.SetAsync(nameof(CacheKeys.Criminals), criminalResponses, TimeSpan.FromMinutes(15));

        return criminalResponses;
    }
}
