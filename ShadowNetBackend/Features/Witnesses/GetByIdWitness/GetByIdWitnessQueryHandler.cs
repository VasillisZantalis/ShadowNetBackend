using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ShadowNetBackend.Common;
using ShadowNetBackend.Features.Witnesses.Common;
using ShadowNetBackend.Infrastructure.Data;
using ShadowNetBackend.Infrastructure.Interfaces;
using ShadowNetBackend.Mappings;

namespace ShadowNetBackend.Features.Witnesses.GetByIdWitness;

public class GetByIdWitnessQueryHandler : IRequestHandler<GetByIdWitnessQuery, WitnessResponse>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly RedisCacheSettings _cacheSettings;
    private readonly ICacheService _cache;

    public GetByIdWitnessQueryHandler(ApplicationDbContext dbContext, ICacheService cache, IOptions<RedisCacheSettings> cacheSettings)
    {
        _dbContext = dbContext;
        _cache = cache;
        _cacheSettings = cacheSettings.Value;
    }

    public async Task<WitnessResponse> Handle(GetByIdWitnessQuery request, CancellationToken cancellationToken)
    {
        string cacheKey = $"{CacheKeys.Witness}_{request.Id}";

        var cachedWitness = await _cache.GetDataAsync<WitnessResponse>(cacheKey);
        if (cachedWitness is not null)
        {
            return cachedWitness;
        }

        var witness = await _dbContext.Witnesses
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

        if (witness is null)
            throw new WitnessNotFoundException();

        var witnessResponse = witness.ToWitnessResponse();

        await _cache.SetAsync(cacheKey, witnessResponse, TimeSpan.FromSeconds(_cacheSettings.DefaultSlidingExpiration));

        return witnessResponse;
    }
}
