using ShadowNetBackend.Features.Witnesses.Common;

namespace ShadowNetBackend.Features.Witnesses.GetAllWitnesses;

public class GetWitnessesQueryHandler : IRequestHandler<GetWitnessesQuery, IEnumerable<WitnessResponse>>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ICacheService _cache;

    public GetWitnessesQueryHandler(ApplicationDbContext dbContext, ICacheService cache)
    {
        _dbContext = dbContext;
        _cache = cache;
    }

    public async Task<IEnumerable<WitnessResponse>> Handle(GetWitnessesQuery request, CancellationToken cancellationToken)
    {
        var cachedWitnesses = await _cache.GetDataAsync<List<WitnessResponse>>(nameof(CacheKeys.Witnesses));
        if (cachedWitnesses is not null)
        {
            return cachedWitnesses;
        }

        var query = _dbContext.Witnesses.AsQueryable()
            .ApplySorting(request.Parameters.OrderBy)
            .ApplyPagination(request.Parameters.PageSize, request.Parameters.PageNumber);

        if (!string.IsNullOrEmpty(request.Parameters.Name))
        {
            query = query.Where(w => request.Parameters.Name.Contains(w.FirstName) || request.Parameters.Name.Contains(w.LastName));
        }

        if (!string.IsNullOrEmpty(request.Parameters.Alias))
        {
            query = query.Where(w => w.Alias != null
                && (request.Parameters.Alias.Contains(w.Alias)
                    || request.Parameters.Alias.Contains(w.Alias)));
        }

        var witnesses = await query.ToListAsync(cancellationToken);
        var witnessResponses = witnesses.Select(s => s.ToWitnessResponse()).ToList();

        await _cache.SetAsync(nameof(CacheKeys.Witnesses), witnessResponses, TimeSpan.FromMinutes(15));

        return witnessResponses;
    }
}
