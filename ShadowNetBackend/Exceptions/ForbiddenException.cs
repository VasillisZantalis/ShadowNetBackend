namespace ShadowNetBackend.Exceptions;

public class ForbiddenException : BaseException
{
    public ForbiddenException(string message = "You do not have permission to access this resource.")
        : base(message, StatusCodes.Status403Forbidden) { }
}
