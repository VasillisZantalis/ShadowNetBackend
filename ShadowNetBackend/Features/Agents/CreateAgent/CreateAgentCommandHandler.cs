using MediatR;
using Microsoft.AspNetCore.Identity;
using ShadowNetBackend.Features.Agents.GetAllAgents;
using ShadowNetBackend.Infrastructure.Data;

namespace ShadowNetBackend.Features.Agents.CreateAgent;

public class CreateAgentCommandHandler : IRequestHandler<CreateAgentCommand, Guid?>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly UserManager<Agent> _userManager;

    public CreateAgentCommandHandler(UserManager<Agent> userManager, ApplicationDbContext dbContext)
    {
        _userManager = userManager;
        _dbContext = dbContext;
    }

    public async Task<Guid?> Handle(CreateAgentCommand request, CancellationToken cancellationToken)
    {
        var user = new Agent
        {
            UserName = request.Email,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Rank = request.Rank,
            Alias = request.Alias,
            Specialization = request.Specialization,
            ClearanceLevel = request.ClearanceLevel
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
            return null;

        await _userManager.AddToRoleAsync(user, request.Rank.ToString());

        _dbContext.Agents.Attach(user);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Guid.Parse(user.Id);
    }
}
