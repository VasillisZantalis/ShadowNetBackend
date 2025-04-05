using ShadowNetBackend.Features.Criminals.GetByIdCriminal;

namespace ShadowNetBackend.Features.Criminals.DeleteCriminal;

public class DeleteCriminalCommandHandler : IRequestHandler<DeleteCriminalCommand, bool>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ISender _sender;
    private readonly ICacheService _cache;

    public DeleteCriminalCommandHandler(ApplicationDbContext dbContext, ISender sender, ICacheService cache)
    {
        _dbContext = dbContext;
        _sender = sender;
        _cache = cache;
    }

    public async Task<bool> Handle(DeleteCriminalCommand request, CancellationToken cancellationToken)
    {
        await _sender.Send(new GetByIdCriminalQuery(request.Id), cancellationToken);
        var criminal = await _dbContext.Criminals.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        _dbContext.Criminals.Remove(criminal!);
        await _dbContext.SaveChangesAsync(cancellationToken);

        await _cache.RemoveAsync(nameof(CacheKeys.Criminals));
        return true;
    }
}