using ShadowNetBackend.Features.Agents.GetByIdAgent;

namespace ShadowNetBackend.Features.Messages.CreateMessage;

public class CreateMessageCommandHandler : IRequestHandler<CreateMessageCommand, Guid>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ISender _sender;
    private readonly ICryptographyService _cryptographyService;
    private readonly ICacheService _cache;

    public CreateMessageCommandHandler(ApplicationDbContext dbContext, ISender sender, ICryptographyService cryptographyService, ICacheService cache)
    {
        _dbContext = dbContext;
        _sender = sender;
        _cryptographyService = cryptographyService;
        _cache = cache;
    }

    public async Task<Guid> Handle(CreateMessageCommand request, CancellationToken cancellationToken)
    {
        await _sender.Send(new GetByIdAgentQuery(request.SenderId), cancellationToken);
        await _sender.Send(new GetByIdAgentQuery(request.ReceiverId), cancellationToken);

        var message = new Message
        {
            SenderId = request.SenderId,
            ReceiverId = request.ReceiverId,
            Title = request.Title,
            Content = _cryptographyService.Encrypt(request.Content, EncryptionType.AES),
            SentAt = DateTime.UtcNow,
            ExpiresAt = request.AutoDestructTime.HasValue ? DateTimeOffset.UtcNow.Add(request.AutoDestructTime.Value) : null,
            IsDestroyed = false
        };

        _dbContext.Messages.Add(message);
        await _dbContext.SaveChangesAsync(cancellationToken);

        await _cache.RemoveAsync(nameof(CacheKeys.Messages));

        return message.Id;
    }
}
