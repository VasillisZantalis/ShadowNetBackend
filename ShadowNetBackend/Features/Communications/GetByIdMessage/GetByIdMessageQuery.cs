namespace ShadowNetBackend.Features.Communications.GetByIdMessage;

public record GetByIdMessageQuery(Guid Id) : IRequest<MessageResponse>;
