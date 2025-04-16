using ShadowNetBackend.Features.Agents.Common;

namespace ShadowNetBackend.Features.Agents.GetByIdAgent;

public record GetAgentByIdQuery(Guid Id) : IQuery<AgentDto>;

internal class GetAgentByIdHandler(ApplicationDbContext dbContext) : IQueryHandler<GetAgentByIdQuery, AgentDto>
{
    public async Task<AgentDto> Handle(GetAgentByIdQuery request, CancellationToken cancellationToken)
    {
        var agent = await dbContext.Agents
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == request.Id.ToString(), cancellationToken);

        if (agent is null)
            throw new AgentNotFoundException();

        return agent.ToAgentDto();
    }
}