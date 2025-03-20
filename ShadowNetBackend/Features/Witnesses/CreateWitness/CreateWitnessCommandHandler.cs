using ShadowNetBackend.Common.Helpers;

namespace ShadowNetBackend.Features.Witnesses.CreateWitness;

public class CreateWitnessCommandHandler : IRequestHandler<CreateWitnessCommand, Guid?>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ICacheService _cache;

    public CreateWitnessCommandHandler(ApplicationDbContext dbContext, ICacheService cache)
    {
        _dbContext = dbContext;
        _cache = cache;
    }

    public async Task<Guid?> Handle(CreateWitnessCommand request, CancellationToken cancellationToken)
    {
        var witness = new Witness
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Alias = request.Alias ?? string.Empty,
            Image = request.Image is null
                ? null
                : FileHelper.ConvertFromBase64(request.Image),
            RiskLevel = request.RiskLevel,
            LocationHistory = request.LocationHistory,
            RelocationStatus = request.RelocationStatus
        };

        _dbContext.Witnesses.Add(witness);
        await _dbContext.SaveChangesAsync(cancellationToken);

        await _cache.RemoveAsync($"{CacheKeys.Witnesses}");

        return witness.Id;
    }
}
