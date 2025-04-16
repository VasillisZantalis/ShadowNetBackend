namespace ShadowNetBackend.Exceptions;

public class ValidationFailedException : BaseException
{
    public Dictionary<string, string[]> Errors { get; }

    public ValidationFailedException(Dictionary<string, string[]> errors)
        : base("One or more validation errors occurred.", StatusCodes.Status400BadRequest)
    {
        Errors = errors;
    }
}
