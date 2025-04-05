using ShadowNetBackend.Features.Witnesses.Common;

namespace ShadowNetBackend.Features.Witnesses.DeleteWitness;

public class DeleteWitnessCommandHandler : IRequestHandler<DeleteWitnessCommand, bool>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ICacheService _cache;

    public DeleteWitnessCommandHandler(ApplicationDbContext dbContext, ICacheService cache)
    {
        _dbContext = dbContext;
        _cache = cache;
    }

    public async Task<bool> Handle(DeleteWitnessCommand request, CancellationToken cancellationToken)
    {
        var witness = await _dbContext.Witnesses.FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);
        if (witness is null)
            throw new WitnessNotFoundException();

        _dbContext.Witnesses.Remove(witness);
        await _dbContext.SaveChangesAsync(cancellationToken);

        await _cache.RemoveAsync(nameof(CacheKeys.Witnesses));

        return true;
    }
}
