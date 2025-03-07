using MediatR;
using Microsoft.EntityFrameworkCore;
using ShadowNetBackend.Exceptions;
using ShadowNetBackend.Extensions;
using ShadowNetBackend.Infrastructure.Data;

namespace ShadowNetBackend.Features.Agents.DeleteAgent;

public class DeleteAgentCommandHandler : IRequestHandler<DeleteAgentCommand, bool>
{
    private readonly ApplicationDbContext _dbContext;

    public DeleteAgentCommandHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> Handle(DeleteAgentCommand request, CancellationToken cancellationToken)
    {
        if (!await _dbContext.ExistsAsync<Agent>(request.Id.ToString(), cancellationToken))
        {
            throw new NotFoundException($"Agent with id {request.Id} was not found");
        }

        var agent = await _dbContext.Agents.FirstAsync(a => a.Id == request.Id.ToString(), cancellationToken);

        _dbContext.Agents.Remove(agent);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return true;
    }
}
