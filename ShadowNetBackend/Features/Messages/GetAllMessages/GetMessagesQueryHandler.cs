using ShadowNetBackend.Features.Messages.Common;

namespace ShadowNetBackend.Features.Messages.GetAllMessages;

public class GetMessagesQueryHandler : IRequestHandler<GetMessagesQuery, IEnumerable<MessageResponse>>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ICacheService _cache;

    public GetMessagesQueryHandler(ApplicationDbContext dbContext, ICacheService cache)
    {
        _dbContext = dbContext;
        _cache = cache;
    }

    public async Task<IEnumerable<MessageResponse>> Handle(GetMessagesQuery request, CancellationToken cancellationToken)
    {
        var cachedMessages = await _cache.GetDataAsync<List<MessageResponse>>(nameof(CacheKeys.Messages));
        if (cachedMessages is not null)
        {
            return cachedMessages;
        }

        var query = _dbContext.Messages.AsQueryable()
            .Where(w => w.IsDestroyed == false)
            .ApplySorting(request.Parameters.OrderBy)
            .ApplyPagination(request.Parameters.PageSize, request.Parameters.PageNumber);

        if (request.Parameters.FromDate.HasValue)
            query = query.Where(w => w.SentAt >= request.Parameters.FromDate.Value);

        if (request.Parameters.ToDate.HasValue)
            query = query.Where(w => w.SentAt <= request.Parameters.ToDate.Value);

        if (request.Parameters.SenderId.HasValue)
            query = query.Where(w => w.SenderId == request.Parameters.SenderId.Value);

        if (request.Parameters.RecipientId.HasValue)
            query = query.Where(w => w.SenderId == request.Parameters.RecipientId.Value);

        if (!string.IsNullOrWhiteSpace(request.Parameters.Text))
            query = query.Where(w => w.Content.Contains(request.Parameters.Text)
                                || w.Title.Contains(request.Parameters.Text));

        var messages = await query.ToListAsync(cancellationToken);

        var messageResponses = messages.Select(message => new MessageResponse
        (
            message.Id,
            message.SenderId,
            message.ReceiverId,
            message.Title,
            message.Content,
            message.SentAt
        )).ToList();

        await _cache.SetAsync(nameof(CacheKeys.Messages), messageResponses, TimeSpan.FromHours(1));

        return messageResponses;
    }
}
