using MediatR;
using Microsoft.EntityFrameworkCore;
using ShadowNetBackend.Features.Agents.Common;
using ShadowNetBackend.Infrastructure.Data;
using ShadowNetBackend.Mappings;

namespace ShadowNetBackend.Features.Agents.GetAllAgents;

public class GetAgentsQueryHandler : IRequestHandler<GetAgentsQuery, IEnumerable<AgentResponse>>
{
    private readonly ApplicationDbContext _dbContext;

    public GetAgentsQueryHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<AgentResponse>> Handle(GetAgentsQuery request, CancellationToken cancellationToken)
    {
        var query = _dbContext.Agents.AsQueryable();

        if (!string.IsNullOrEmpty(request.Parameters.Name))
        {
            query = query.Where(w => request.Parameters.Name.Contains(w.FirstName) || request.Parameters.Name.Contains(w.LastName));
        }

        if (request.Parameters.PageSize.HasValue && request.Parameters.PageNumber.HasValue)
        {
            int pageSize = request.Parameters.PageSize.Value;
            int pageNumber = request.Parameters.PageNumber.Value;

            query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }

        var agents = await query.ToListAsync(cancellationToken);

        return agents.Select(agent => agent.ToAgentResponse()).ToList();
    }
}
