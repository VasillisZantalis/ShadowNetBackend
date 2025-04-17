using ShadowNetBackend.Dtos.Messages;
using ShadowNetBackend.Features.Agents.Common;

namespace ShadowNetBackend.Features.Messages.CreateMessage;

public record CreateMessageCommand(MessageForCreationDto MessageForCreation) : ICommand<Guid>;

public class CreateMessageCommandValidator : AbstractValidator<CreateMessageCommand>
{
    public CreateMessageCommandValidator()
    {
        RuleFor(x => x.MessageForCreation.SenderId).NotEmpty().WithMessage("Sender is required");

        RuleFor(x => x.MessageForCreation.ReceiverId).NotEmpty().WithMessage("Receiver is required");

        RuleFor(x => x.MessageForCreation.Title).NotEmpty();

        RuleFor(x => x.MessageForCreation.Content).NotEmpty();
    }
}

internal class CreateMessageHandler(
    ApplicationDbContext dbContext,
    ICryptographyService cryptographyService,
    ICacheService cache) : ICommandHandler<CreateMessageCommand, Guid>
{
    public async Task<Guid> Handle(CreateMessageCommand request, CancellationToken cancellationToken)
    {
        if (!await dbContext.ExistsAsync<Agent>(request.MessageForCreation.SenderId.ToString(), cancellationToken)
            || !await dbContext.ExistsAsync<Agent>(request.MessageForCreation.ReceiverId.ToString(), cancellationToken))
        {
            throw new AgentNotFoundException();
        }

        var message = new Message
        {
            SenderId = request.MessageForCreation.SenderId,
            ReceiverId = request.MessageForCreation.ReceiverId,
            Title = request.MessageForCreation.Title,
            Content = cryptographyService.Encrypt(request.MessageForCreation.Content, EncryptionType.AES),
            SentAt = DateTime.UtcNow,
            ExpiresAt = request.MessageForCreation.AutoDestructTime.HasValue ? DateTimeOffset.UtcNow.Add(request.MessageForCreation.AutoDestructTime.Value) : null,
            IsDestroyed = false
        };

        dbContext.Messages.Add(message);
        await dbContext.SaveChangesAsync(cancellationToken);

        await cache.RemoveAsync(nameof(CacheKeys.Messages));

        return message.Id;
    }
}
