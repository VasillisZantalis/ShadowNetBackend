namespace ShadowNetBackend.Exceptions;

public class NotFoundException : BaseException
{
    public NotFoundException(string message = "The requested resource was not found.")
        : base(message, StatusCodes.Status404NotFound) { }
}
