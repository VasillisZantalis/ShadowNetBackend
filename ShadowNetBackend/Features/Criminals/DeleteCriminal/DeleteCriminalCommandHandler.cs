using ShadowNetBackend.Features.Criminals.Common;

namespace ShadowNetBackend.Features.Criminals.DeleteCriminal;

public class DeleteCriminalCommandHandler : IRequestHandler<DeleteCriminalCommand, bool>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ICacheService _cache;

    public DeleteCriminalCommandHandler(ApplicationDbContext dbContext, ICacheService cache)
    {
        _dbContext = dbContext;
        _cache = cache;
    }

    public async Task<bool> Handle(DeleteCriminalCommand request, CancellationToken cancellationToken)
    {
        var criminal = await _dbContext.Criminals.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
        if (criminal is null)
            throw new CriminalNotFoundException();

        _dbContext.Criminals.Remove(criminal);
        await _dbContext.SaveChangesAsync(cancellationToken);

        await _cache.RemoveAsync(nameof(CacheKeys.Criminals));
        return true;
    }
}