namespace ShadowNetBackend.Exceptions;

public class BadRequestException : BaseException
{
    public BadRequestException(string message = "The request is invalid.")
        : base(message, StatusCodes.Status400BadRequest) { }
}
