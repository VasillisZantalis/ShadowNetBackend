using MediatR;
using Microsoft.EntityFrameworkCore;
using ShadowNetBackend.Exceptions;
using ShadowNetBackend.Features.Agents.Common;
using ShadowNetBackend.Infrastructure.Data;
using ShadowNetBackend.Mappings;

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

        return agent is null
            ? throw new NotFoundException("Agent not found")
            : agent.ToAgentResponse();
    }
}
