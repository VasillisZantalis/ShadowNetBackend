using ShadowNetBackend.Features.SafeHouses.Common;

namespace ShadowNetBackend.Features.SafeHouses.UpdateSafeHouse;

public class UpdateSafeHouseCommandHandler : IRequestHandler<UpdateSafeHouseCommand, bool>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ICacheService _cache;

    public UpdateSafeHouseCommandHandler(ApplicationDbContext dbContext, ICacheService cache)
    {
        _dbContext = dbContext;
        _cache = cache;
    }

    public async Task<bool> Handle(UpdateSafeHouseCommand request, CancellationToken cancellationToken)
    {
        var safeHouse = await _dbContext.SafeHouses.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (safeHouse is null)
            throw new SafeHouseNotFoundException();

        safeHouse.Location = request.Location;
        safeHouse.Capacity = request.Capacity;
        safeHouse.IsActive = request.IsActive;

        _dbContext.SafeHouses.Update(safeHouse);
        await _dbContext.SaveChangesAsync(cancellationToken);

        await _cache.RemoveAsync(nameof(CacheKeys.SafeHouses));

        return true;
    }
}
