using ShadowNetBackend.Features.Agents.Common;

namespace ShadowNetBackend.Features.Agents.GetAllAgents;

public class GetAgentsQueryHandler : IRequestHandler<GetAgentsQuery, IEnumerable<AgentResponse>>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ICacheService _cache;

    public GetAgentsQueryHandler(ApplicationDbContext dbContext, ICacheService cache)
    {
        _dbContext = dbContext;
        _cache = cache;
    }

    public async Task<IEnumerable<AgentResponse>> Handle(GetAgentsQuery request, CancellationToken cancellationToken)
    {
        var cachedAgents = await _cache.GetDataAsync<List<AgentResponse>>(nameof(CacheKeys.Agents));
        if (cachedAgents is not null)
        {
            return cachedAgents;
        }

        var query = _dbContext.Agents.AsQueryable()
            .ApplySorting(request.Parameters.OrderBy)
            .ApplyPagination(request.Parameters.PageSize, request.Parameters.PageNumber);

        if (!string.IsNullOrEmpty(request.Parameters.Name))
            query = query.Where(w => w.FirstName.Contains(request.Parameters.Name) || w.LastName.Contains(request.Parameters.Name));

        var agents = await query.ToListAsync(cancellationToken);
        var agentResponses = agents.Select(s => s.ToAgentResponse()).ToList();

        await _cache.SetAsync(nameof(CacheKeys.Agents), agentResponses, TimeSpan.FromMinutes(15));

        return agentResponses;
    }
}
