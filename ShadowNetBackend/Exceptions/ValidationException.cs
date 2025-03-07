namespace ShadowNetBackend.Exceptions;

public class ValidationException : BaseException
{
    public Dictionary<string, string[]> Errors { get; }

    public ValidationException(Dictionary<string, string[]> errors)
        : base("One or more validation errors occurred.", StatusCodes.Status400BadRequest)
    {
        Errors = errors;
    }
}
