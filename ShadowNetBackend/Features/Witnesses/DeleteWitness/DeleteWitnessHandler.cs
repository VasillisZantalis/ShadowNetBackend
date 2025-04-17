using ShadowNetBackend.Features.Witnesses.Common;

namespace ShadowNetBackend.Features.Witnesses.DeleteWitness;

public record DeleteWitnessCommand(Guid Id) : ICommand<bool>;

internal class DeleteWitnessHandler(
    ApplicationDbContext dbContext,
    ICacheService cache) : ICommandHandler<DeleteWitnessCommand, bool>
{
    public async Task<bool> Handle(DeleteWitnessCommand request, CancellationToken cancellationToken)
    {
        var witness = await dbContext.Witnesses.FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);
        if (witness is null)
            throw new WitnessNotFoundException();

        dbContext.Witnesses.Remove(witness);
        await dbContext.SaveChangesAsync(cancellationToken);

        await cache.RemoveAsync(nameof(CacheKeys.Witnesses));
        return true;
    }
}