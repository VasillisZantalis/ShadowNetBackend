namespace ShadowNetBackend.Features.Communications.GetByIdMessage;

public class GetByIdMessageQueryHandler : IRequestHandler<GetByIdMessageQuery, MessageResponse>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ICryptographyService _cryptographyService;
    private readonly ICacheService _cache;

    public GetByIdMessageQueryHandler(ApplicationDbContext dbContext, ICryptographyService cryptographyService, ICacheService cache)
    {
        _dbContext = dbContext;
        _cryptographyService = cryptographyService;
        _cache = cache;
    }

    public async Task<MessageResponse> Handle(GetByIdMessageQuery request, CancellationToken cancellationToken)
    {
        string cacheKey = $"{CacheKeys.Messages}_{request.Id}";

        var cachedMessage = await _cache.GetDataAsync<MessageResponse>(cacheKey);
        if (cachedMessage is not null)
        {
            return cachedMessage;
        }

        var message = await _dbContext.Messages
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (message is null)
            throw new MessageNotFoundException();

        var messageResponse = new MessageResponse
        (
            message.Id,
            message.SenderId,
            message.ReceiverId,
            message.Title,
            _cryptographyService.Decrypt(message.Content, EncryptionType.AES),
            message.SentAt
        );

        await _cache.SetAsync(cacheKey, messageResponse, TimeSpan.FromHours(1));

        return messageResponse;
    }
}
