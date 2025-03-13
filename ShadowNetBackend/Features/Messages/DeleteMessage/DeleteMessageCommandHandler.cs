using ShadowNetBackend.Features.Messages.GetByIdMessage;

namespace ShadowNetBackend.Features.Messages.DeleteMessage;

public class DeleteMessageCommandHandler : IRequestHandler<DeleteMessageCommand, bool>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ISender _sender;
    private readonly ICacheService _cache;

    public DeleteMessageCommandHandler(ApplicationDbContext dbContext, ISender sender, ICacheService cache)
    {
        _dbContext = dbContext;
        _sender = sender;
        _cache = cache;
    }

    public async Task<bool> Handle(DeleteMessageCommand request, CancellationToken cancellationToken)
    {
        string cacheKey = $"{CacheKeys.Messages}_{request.Id}";

        await _sender.Send(new GetByIdMessageQuery(request.Id), cancellationToken);

        var message = await _dbContext.Messages.FindAsync(request.Id);

        _dbContext.Messages.Remove(message!);
        await _dbContext.SaveChangesAsync(cancellationToken);

        await _cache.RemoveAsync(cacheKey);

        return true;
    }
}
