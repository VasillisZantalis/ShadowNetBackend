using ShadowNetBackend.Features.Criminals.Common;

namespace ShadowNetBackend.Features.Criminals.DeleteCriminal;

public record DeleteCriminalCommand(Guid Id) : ICommand<bool>;

internal class DeleteCriminalCommandHandler(
    ApplicationDbContext dbContext,
    ICacheService cache) : ICommandHandler<DeleteCriminalCommand, bool>
{
    public async Task<bool> Handle(DeleteCriminalCommand request, CancellationToken cancellationToken)
    {
        var criminal = await dbContext.Criminals.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
        if (criminal is null)
            throw new CriminalNotFoundException();

        dbContext.Criminals.Remove(criminal);
        await dbContext.SaveChangesAsync(cancellationToken);

        await cache.RemoveAsync(nameof(CacheKeys.Criminals));
        return true;
    }
}