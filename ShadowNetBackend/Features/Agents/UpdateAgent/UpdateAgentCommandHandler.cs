using MediatR;
using Microsoft.EntityFrameworkCore;
using ShadowNetBackend.Exceptions;
using ShadowNetBackend.Extensions;
using ShadowNetBackend.Features.Missions.GetByIdMission;
using ShadowNetBackend.Infrastructure.Data;

namespace ShadowNetBackend.Features.Agents.UpdateAgent;

public class UpdateAgentCommandHandler : IRequestHandler<UpdateAgentCommand, bool>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IMediator _mediator;

    public UpdateAgentCommandHandler(ApplicationDbContext dbContext, IMediator mediator)
    {
        _dbContext = dbContext;
        _mediator = mediator;
    }

    public async Task<bool> Handle(UpdateAgentCommand request, CancellationToken cancellationToken)
    {
        if (!await _dbContext.ExistsAsync<Agent>(request.Id.ToString(), cancellationToken))
            throw new NotFoundException($"Agent with id {request.Id} was not found");

        if (request.MissionId.HasValue)
            await _mediator.Send(new GetByIdMissionQuery(request.MissionId.Value, null), cancellationToken);

        var agent = await _dbContext.Agents.FirstAsync(a => a.Id == request.Id.ToString(), cancellationToken);

        agent.FirstName = request.FirstName;
        agent.LastName = request.LastName;
        agent.Alias = request.Alias;
        agent.Specialization = request.Specialization;
        agent.ClearanceLevel = request.ClearanceLevel;
        agent.MissionId = request.MissionId;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}
