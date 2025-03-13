using ShadowNetBackend.Features.SafeHouses.GetByIdSafeHouse;

namespace ShadowNetBackend.Features.SafeHouses.DeleteSafeHouse;

public class DeleteSafeHouseCommandHandler : IRequestHandler<DeleteSafeHouseCommand, bool>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ISender _sender;
    private readonly ICacheService _cache;

    public DeleteSafeHouseCommandHandler(ApplicationDbContext dbContext, ISender sender, ICacheService cache)
    {
        _dbContext = dbContext;
        _sender = sender;
        _cache = cache;
    }

    public async Task<bool> Handle(DeleteSafeHouseCommand request, CancellationToken cancellationToken)
    {
        string cacheKey = $"{CacheKeys.SafeHouses}_{request.Id}";

        await _sender.Send(new GetByIdSafeHouseQuery(request.Id), cancellationToken);

        var SafeHouse = await _dbContext.SafeHouses.FindAsync(request.Id);

        _dbContext.SafeHouses.Remove(SafeHouse!);
        await _dbContext.SaveChangesAsync(cancellationToken);

        await _cache.RemoveAsync(cacheKey);

        return true;
    }
}
