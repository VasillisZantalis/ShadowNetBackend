using ShadowNetBackend.Features.SafeHouses.Common;

namespace ShadowNetBackend.Features.SafeHouses.DeleteSafeHouse;

public class DeleteSafeHouseCommandHandler : IRequestHandler<DeleteSafeHouseCommand, bool>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ICacheService _cache;

    public DeleteSafeHouseCommandHandler(ApplicationDbContext dbContext, ICacheService cache)
    {
        _dbContext = dbContext;
        _cache = cache;
    }

    public async Task<bool> Handle(DeleteSafeHouseCommand request, CancellationToken cancellationToken)
    {
        var safeHouse = await _dbContext.SafeHouses.FindAsync(request.Id);
        if (safeHouse is null)
            throw new SafeHouseNotFoundException();

        _dbContext.SafeHouses.Remove(safeHouse);
        await _dbContext.SaveChangesAsync(cancellationToken);

        await _cache.RemoveAsync(nameof(CacheKeys.SafeHouses));

        return true;
    }
}
