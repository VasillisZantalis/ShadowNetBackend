using ShadowNetBackend.Features.Agents.Common;

namespace ShadowNetBackend.Features.Agents.GetByIdAgent;

public class GetByIdAgentQueryHandler : IRequestHandler<GetByIdAgentQuery, AgentResponse>
{
    private readonly ApplicationDbContext _dbContext;

    public GetByIdAgentQueryHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<AgentResponse> Handle(GetByIdAgentQuery request, CancellationToken cancellationToken)
    {
        var agent = await _dbContext.Agents
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == request.Id.ToString(), cancellationToken);

        if (agent is null)
            throw new AgentNotFoundException();

        var agentResponse = agent.ToAgentResponse();

        return agentResponse;
    }
}
