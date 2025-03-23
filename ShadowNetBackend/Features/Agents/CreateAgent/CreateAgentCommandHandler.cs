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
        var user = new Agent
        {
            UserName = request.Email,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Image = request.Image is null
                ? null
                : FileHelper.ConvertFromBase64(request.Image),
            Rank = request.Rank,
            Alias = request.Alias,
            Specialization = request.Specialization,
            ClearanceLevel = request.ClearanceLevel
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            var errors = result.Errors
                   .GroupBy(e => e.Code)
                   .ToDictionary(
                       g => g.Key,
                       g => g.Select(e => e.Description).ToArray()
                   );
            throw new ValidationException(errors);
        }

        await _userManager.AddToRoleAsync(user, request.Rank.ToString());
        await _dbContext.SaveChangesAsync(cancellationToken);

        await _cache.RemoveAsync(nameof(CacheKeys.Agents));

        return Guid.Parse(user.Id);
    }
}
