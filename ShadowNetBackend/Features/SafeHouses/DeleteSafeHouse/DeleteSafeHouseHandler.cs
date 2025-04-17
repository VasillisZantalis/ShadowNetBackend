using ShadowNetBackend.Features.SafeHouses.Common;

namespace ShadowNetBackend.Features.SafeHouses.DeleteSafeHouse;

public record DeleteSafeHouseCommand(int Id) : ICommand<bool>;

internal class DeleteSafeHouseHandler(ApplicationDbContext dbContext, ICacheService cache) : ICommandHandler<DeleteSafeHouseCommand, bool>
{
    public async Task<bool> Handle(DeleteSafeHouseCommand request, CancellationToken cancellationToken)
    {
        var safeHouse = await dbContext.SafeHouses.FindAsync(request.Id);
        if (safeHouse is null)
            throw new SafeHouseNotFoundException();

        dbContext.SafeHouses.Remove(safeHouse);
        await dbContext.SaveChangesAsync(cancellationToken);

        await cache.RemoveAsync(nameof(CacheKeys.SafeHouses));
        return true;
    }
}