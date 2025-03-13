using ShadowNetBackend.Features.Messages.Common;

namespace ShadowNetBackend.Features.Messages.GetByIdMessage;

public record GetByIdMessageQuery(Guid Id) : IRequest<MessageResponse>;
