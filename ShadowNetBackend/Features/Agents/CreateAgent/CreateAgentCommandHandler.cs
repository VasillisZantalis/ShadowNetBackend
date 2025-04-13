using Microsoft.AspNetCore.Identity;
using ShadowNetBackend.Exceptions;

namespace ShadowNetBackend.Features.Agents.CreateAgent;

public class CreateAgentCommandHandler : IRequestHandler<CreateAgentCommand, Guid?>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly UserManager<Agent> _userManager;
    private readonly ICacheService _cache;

    public CreateAgentCommandHandler(UserManager<Agent> userManager, ApplicationDbContext dbContext, ICacheService cache)
    {
        _userManager = userManager;
        _dbContext = dbContext;
        _cache = cache;
    }

    public async Task<Guid?> Handle(CreateAgentCommand request, CancellationToken cancellationToken)
    {
        var agent = request.ToAgent();

        var result = await _userManager.CreateAsync(agent, request.Password);

        if (!result.Succeeded)
        {
            var errors = result.Errors
                   .GroupBy(e => e.Code)
                   .ToDictionary(
                       g => g.Key,
                       g => g.Select(e => e.Description).ToArray()
                   );
            throw new Exceptions.ValidationException(errors);
        }

        await _userManager.AddToRoleAsync(agent, request.Rank.ToString());
        await _dbContext.SaveChangesAsync(cancellationToken);

        await _cache.RemoveAsync(nameof(CacheKeys.Agents));

        return Guid.Parse(agent.Id);
    }
}
