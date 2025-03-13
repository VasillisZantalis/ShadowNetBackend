using ShadowNetBackend.Exceptions;

namespace ShadowNetBackend.Features.Messages.Common;

public class MessageNotFoundException : NotFoundException
{
    public MessageNotFoundException(string message = "Message was not found")
        : base(message) { }
}
