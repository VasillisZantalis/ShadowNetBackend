using FluentValidation;

namespace ShadowNetBackend.Features.Communications.CreateMessage;

public class CreateMessageValidator : AbstractValidator<CreateMessageCommand>
{
    public CreateMessageValidator()
    {
        RuleFor(x => x.SenderId)
            .NotEmpty().WithMessage("Sender is required");

        RuleFor(x => x.ReceiverId)
            .NotEmpty().WithMessage("Receiver is required");

        RuleFor(x => x.Title)
            .NotEmpty();

        RuleFor(x => x.Content)
            .NotEmpty();
    }
}
