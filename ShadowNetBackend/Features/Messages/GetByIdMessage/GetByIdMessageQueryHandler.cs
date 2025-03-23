using ShadowNetBackend.Features.Messages.Common;

namespace ShadowNetBackend.Features.Messages.GetByIdMessage;

public class GetByIdMessageQueryHandler : IRequestHandler<GetByIdMessageQuery, MessageResponse>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ICryptographyService _cryptographyService;

    public GetByIdMessageQueryHandler(ApplicationDbContext dbContext, ICryptographyService cryptographyService)
    {
        _dbContext = dbContext;
        _cryptographyService = cryptographyService;
    }

    public async Task<MessageResponse> Handle(GetByIdMessageQuery request, CancellationToken cancellationToken)
    {
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

        return messageResponse;
    }
}
