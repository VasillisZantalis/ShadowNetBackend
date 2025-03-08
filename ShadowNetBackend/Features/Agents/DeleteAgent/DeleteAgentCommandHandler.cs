using MediatR;
using Microsoft.EntityFrameworkCore;
using ShadowNetBackend.Features.Agents.GetByIdAgent;
using ShadowNetBackend.Infrastructure.Data;

namespace ShadowNetBackend.Features.Agents.DeleteAgent;

public class DeleteAgentCommandHandler : IRequestHandler<DeleteAgentCommand, bool>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ISender _sender;

    public DeleteAgentCommandHandler(ApplicationDbContext dbContext, ISender sender)
    {
        _dbContext = dbContext;
        _sender = sender;
    }

    public async Task<bool> Handle(DeleteAgentCommand request, CancellationToken cancellationToken)
    {
        await _sender.Send(new GetByIdAgentQuery(request.Id), cancellationToken);

        var agent = await _dbContext.Agents.FirstOrDefaultAsync(a => a.Id == request.Id.ToString(), cancellationToken);

        _dbContext.Agents.Remove(agent!);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}
