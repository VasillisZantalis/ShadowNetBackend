namespace ShadowNetBackend.Features.Messages.GetByIdMessage;

public record GetMessageByIdQuery(Guid Id) : IQuery<MessageDto>;

internal class GetMessageByIdHandler(
    ApplicationDbContext dbContext) : IQueryHandler<GetMessageByIdQuery, MessageDto>
{
    public async Task<MessageDto> Handle(GetMessageByIdQuery request, CancellationToken cancellationToken)
    {
        var message = await dbContext.Messages
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (message is null)
            throw new MessageNotFoundException();

        var messageResponse = new MessageDto
        (
            message.Id,
            message.SenderId,
            message.ReceiverId,
            message.Title,
            message.Content,
            message.SentAt
        );

        return messageResponse;
    }
}
