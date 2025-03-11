using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ShadowNetBackend.Common;
using ShadowNetBackend.Extensions;
using ShadowNetBackend.Features.Agents.Common;
using ShadowNetBackend.Infrastructure.Data;
using ShadowNetBackend.Infrastructure.Interfaces;
using ShadowNetBackend.Mappings;

namespace ShadowNetBackend.Features.Agents.GetAllAgents;

public class GetAgentsQueryHandler : IRequestHandler<GetAgentsQuery, IEnumerable<AgentResponse>>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly RedisCacheSettings _cacheSettings;
    private readonly ICacheService _cache;

    public GetAgentsQueryHandler(ApplicationDbContext dbContext, ICacheService cache, IOptions<RedisCacheSettings> cacheSettings)
    {
        _dbContext = dbContext;
        _cache = cache;
        _cacheSettings = cacheSettings.Value;
    }

    public async Task<IEnumerable<AgentResponse>> Handle(GetAgentsQuery request, CancellationToken cancellationToken)
    {
        string cacheKey = "agents";

        var cachedAgents = await _cache.GetDataAsync<List<AgentResponse>>(cacheKey);
        if (cachedAgents is not null)
        {
            return cachedAgents;
        }

        var query = _dbContext.Agents.AsQueryable()
            .ApplySorting(request.Parameters.OrderBy)
            .ApplyPagination(request.Parameters.PageSize, request.Parameters.PageNumber);

        if (!string.IsNullOrEmpty(request.Parameters.Name))
        {
            query = query.Where(w => request.Parameters.Name.Contains(w.FirstName) || request.Parameters.Name.Contains(w.LastName));
        }

        var agents = await query.ToListAsync(cancellationToken);

        var agentResponses = agents.Select(s => s.ToAgentResponse()).ToList();

        await _cache.SetAsync(cacheKey, agentResponses, TimeSpan.FromSeconds(_cacheSettings.DefaultSlidingExpiration));

        return agentResponses;
    }
}
