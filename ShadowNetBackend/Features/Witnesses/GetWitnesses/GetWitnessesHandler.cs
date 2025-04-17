using ShadowNetBackend.Features.Witnesses.Common;

namespace ShadowNetBackend.Features.Witnesses.GetAllWitnesses;

public record GetWitnessesQuery(WitnessParameters Parameters) : ICommand<IEnumerable<WitnessDto>>;

internal class GetWitnessesHandler(
    ApplicationDbContext dbContext,
    ICacheService cache) : ICommandHandler<GetWitnessesQuery, IEnumerable<WitnessDto>>
{
    public async Task<IEnumerable<WitnessDto>> Handle(GetWitnessesQuery request, CancellationToken cancellationToken)
    {
        var cachedWitnesses = await cache.GetDataAsync<List<WitnessDto>>(nameof(CacheKeys.Witnesses));
        if (cachedWitnesses is not null)
            return cachedWitnesses;

        var query = dbContext.Witnesses.AsQueryable()
            .ApplySorting(request.Parameters.OrderBy)
            .ApplyPagination(request.Parameters.PageSize, request.Parameters.PageNumber);

        if (!string.IsNullOrEmpty(request.Parameters.Name))
            query = query.Where(w => w.FirstName.Contains(request.Parameters.Name) || w.LastName.Contains(request.Parameters.Name));

        if (!string.IsNullOrEmpty(request.Parameters.Alias))
            query = query.Where(w => w.Alias != null && w.Alias.Contains(request.Parameters.Alias));

        var witnesses = await query.ToListAsync(cancellationToken);
        var witnessDtos = witnesses.ToWitnessDto();

        await cache.SetAsync(nameof(CacheKeys.Witnesses), witnessDtos, TimeSpan.FromMinutes(15));
        return witnessDtos;
    }
}