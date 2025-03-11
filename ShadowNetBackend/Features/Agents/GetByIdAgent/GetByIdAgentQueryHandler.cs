using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ShadowNetBackend.Common;
using ShadowNetBackend.Features.Agents.Common;
using ShadowNetBackend.Infrastructure.Data;
using ShadowNetBackend.Infrastructure.Interfaces;
using ShadowNetBackend.Mappings;

namespace ShadowNetBackend.Features.Agents.GetByIdAgent;

public class GetByIdAgentQueryHandler : IRequestHandler<GetByIdAgentQuery, AgentResponse>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly RedisCacheSettings _cacheSettings;
    private readonly ICacheService _cache;

    public GetByIdAgentQueryHandler(ApplicationDbContext dbContext, ICacheService cache, IOptions<RedisCacheSettings> cacheSettings)
    {
        _dbContext = dbContext;
        _cache = cache;
        _cacheSettings = cacheSettings.Value;
    }

    public async Task<AgentResponse> Handle(GetByIdAgentQuery request, CancellationToken cancellationToken)
    {
        string cacheKey = $"agent_{request.Id}";

        var cachedAgent = await _cache.GetDataAsync<AgentResponse>(cacheKey);
        if (cachedAgent is not null)
        {
            return cachedAgent;
        }

        var agent = await _dbContext.Agents
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == request.Id.ToString(), cancellationToken);

        if (agent is null)
            throw new AgentNotFoundException();

        var agentResponse = agent.ToAgentResponse();

        await _cache.SetAsync(cacheKey, agentResponse, TimeSpan.FromSeconds(_cacheSettings.DefaultSlidingExpiration));

        return agentResponse;
    }
}
