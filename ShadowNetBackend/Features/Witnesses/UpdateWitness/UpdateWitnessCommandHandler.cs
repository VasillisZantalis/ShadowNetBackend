using ShadowNetBackend.Features.Witnesses.GetByIdWitness;

namespace ShadowNetBackend.Features.Witnesses.UpdateWitness;

public class UpdateWitnessCommandHandler : IRequestHandler<UpdateWitnessCommand, bool>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ISender _sender;
    private readonly ICacheService _cache;

    public UpdateWitnessCommandHandler(ApplicationDbContext dbContext, ISender sender, ICacheService cache)
    {
        _dbContext = dbContext;
        _sender = sender;
        _cache = cache;
    }

    public async Task<bool> Handle(UpdateWitnessCommand request, CancellationToken cancellationToken)
    {
        string cacheKey = $"{CacheKeys.Witnesses}_{request.Id}";

        await _sender.Send(new GetByIdWitnessQuery(request.Id), cancellationToken);

        var witness = await _dbContext.Witnesses.FirstOrDefaultAsync(w => w.Id == request.Id, cancellationToken);

        witness!.Alias = request.Alias ?? string.Empty;
        witness.Image = request.Image is null
                ? null
                : FileHelper.ConvertFromBase64(request.Image);
        witness.RiskLevel = request.RiskLevel;
        witness.LocationHistory = request.LocationHistory;
        witness.RelocationStatus = request.RelocationStatus;

        await _dbContext.SaveChangesAsync(cancellationToken);

        await _cache.RemoveAsync(cacheKey);

        return true;
    }
}
