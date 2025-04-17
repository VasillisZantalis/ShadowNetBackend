
namespace ShadowNetBackend.Features.Messages.GetAllMessages;

public record GetMessagesQuery(MessageParameters Parameters) : IQuery<IEnumerable<MessageDto>>;

internal class GetMessagesHandler(
    ApplicationDbContext dbContext,
    ICacheService cache) : IQueryHandler<GetMessagesQuery, IEnumerable<MessageDto>>
{
    public async Task<IEnumerable<MessageDto>> Handle(GetMessagesQuery request, CancellationToken cancellationToken)
    {
        var cachedMessages = await cache.GetDataAsync<List<MessageDto>>(nameof(CacheKeys.Messages));
        if (cachedMessages is not null)
        {
            return cachedMessages;
        }

        var query = dbContext.Messages.AsQueryable()
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

        var messageDtos = messages.Select(message => new MessageDto
        (
            message.Id,
            message.SenderId,
            message.ReceiverId,
            message.Title,
            message.Content,
            message.SentAt
        )).ToList();

        await cache.SetAsync(nameof(CacheKeys.Messages), messageDtos, TimeSpan.FromHours(1));

        return messageDtos;
    }
}
