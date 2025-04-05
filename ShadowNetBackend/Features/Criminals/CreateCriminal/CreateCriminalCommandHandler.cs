namespace ShadowNetBackend.Features.Criminals.CreateCriminal;

public class CreateCriminalCommandHandler : IRequestHandler<CreateCriminalCommand, Guid?>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ICacheService _cache;

    public CreateCriminalCommandHandler(ApplicationDbContext dbContext, ICacheService cache)
    {
        _dbContext = dbContext;
        _cache = cache;
    }

    public async Task<Guid?> Handle(CreateCriminalCommand request, CancellationToken cancellationToken)
    {
        var criminal = request.ToCriminal();

        _dbContext.Criminals.Add(criminal);
        await _dbContext.SaveChangesAsync(cancellationToken);

        await _cache.RemoveAsync(nameof(CacheKeys.Criminals));

        return criminal.Id;
    }
}
