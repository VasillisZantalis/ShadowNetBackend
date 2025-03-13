using ShadowNetBackend.Exceptions;

namespace ShadowNetBackend.Features.Communications.Common;

public class MessageNotFoundException : NotFoundException
{
    public MessageNotFoundException(string message = "Message was not found")
        : base (message) { }
}
