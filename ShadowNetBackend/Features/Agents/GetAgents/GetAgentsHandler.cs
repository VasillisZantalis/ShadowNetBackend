using ShadowNetBackend.Features.Agents.Common;

namespace ShadowNetBackend.Features.Agents.GetAllAgents;

public record GetAgentsQuery(AgentParameters Parameters) : IQuery<IEnumerable<AgentDto>>;

internal class GetAgentsHandler(
    ApplicationDbContext dbContext,
    ICacheService cache) : IQueryHandler<GetAgentsQuery, IEnumerable<AgentDto>>
{
    public async Task<IEnumerable<AgentDto>> Handle(GetAgentsQuery request, CancellationToken cancellationToken)
    {
        var cachedAgents = await cache.GetDataAsync<List<AgentDto>>(nameof(CacheKeys.Agents));
        if (cachedAgents is not null)
            return cachedAgents;

        var query = dbContext.Agents
            .AsNoTracking()
            .ApplySorting(request.Parameters.OrderBy)
            .ApplyPagination(request.Parameters.PageSize, request.Parameters.PageNumber);

        if (!string.IsNullOrEmpty(request.Parameters.Name))
            query = query.Where(w => w.FirstName.Contains(request.Parameters.Name) || w.LastName.Contains(request.Parameters.Name));

        var agents = await query.ToListAsync(cancellationToken);
        var agentDtos = agents.ToAgentDto();

        await cache.SetAsync(nameof(CacheKeys.Agents), agentDtos, TimeSpan.FromMinutes(15));

        return agentDtos;
    }
}
