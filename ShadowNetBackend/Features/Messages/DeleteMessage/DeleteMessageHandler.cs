namespace ShadowNetBackend.Features.Messages.DeleteMessage;

public record DeleteMessageCommand(Guid Id) : ICommand<bool>;

internal class DeleteMessageHandler(
    ApplicationDbContext dbContext,
    ICacheService cache) : ICommandHandler<DeleteMessageCommand, bool>
{
    public async Task<bool> Handle(DeleteMessageCommand request, CancellationToken cancellationToken)
    {
        var message = await dbContext.Messages.FindAsync(request.Id);
        if (message is null)
            throw new MessageNotFoundException();

        dbContext.Messages.Remove(message);
        await dbContext.SaveChangesAsync(cancellationToken);

        await cache.RemoveAsync(nameof(CacheKeys.Messages));
        return true;
    }
}
