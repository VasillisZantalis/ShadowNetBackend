using ShadowNetBackend.Features.Witnesses.GetByIdWitness;

namespace ShadowNetBackend.Features.Witnesses.DeleteWitness;

public class DeleteWitnessCommandHandler : IRequestHandler<DeleteWitnessCommand, bool>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ISender _sender;
    private readonly ICacheService _cache;

    public DeleteWitnessCommandHandler(ApplicationDbContext dbContext, ISender sender, ICacheService cache)
    {
        _dbContext = dbContext;
        _sender = sender;
        _cache = cache;
    }

    public async Task<bool> Handle(DeleteWitnessCommand request, CancellationToken cancellationToken)
    {
        await _sender.Send(new GetByIdWitnessQuery(request.Id), cancellationToken);

        var witness = await _dbContext.Witnesses.FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

        _dbContext.Witnesses.Remove(witness!);
        await _dbContext.SaveChangesAsync(cancellationToken);

        await _cache.RemoveAsync(nameof(CacheKeys.Witnesses));

        return true;
    }
}
