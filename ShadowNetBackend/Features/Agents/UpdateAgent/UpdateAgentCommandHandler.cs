using MediatR;
using Microsoft.EntityFrameworkCore;
using ShadowNetBackend.Exceptions;
using ShadowNetBackend.Extensions;
using ShadowNetBackend.Infrastructure.Data;

namespace ShadowNetBackend.Features.Agents.UpdateAgent;

public class UpdateAgentCommandHandler : IRequestHandler<UpdateAgentCommand, bool>
{
    private readonly ApplicationDbContext _dbContext;

    public UpdateAgentCommandHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> Handle(UpdateAgentCommand request, CancellationToken cancellationToken)
    {
        if (!await _dbContext.ExistsAsync<Agent>(request.Id.ToString(), cancellationToken))
        {
            throw new NotFoundException($"Agent with id {request.Id} was not found");
        }

        var agent = await _dbContext.Agents.FirstAsync(a => a.Id == request.Id.ToString(), cancellationToken);

        agent.FirstName = request.FirstName;
        agent.LastName = request.LastName;
        agent.Alias = request.Alias;
        agent.Specialization = request.Specialization;
        agent.ClearanceLevel = request.ClearanceLevel;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}
