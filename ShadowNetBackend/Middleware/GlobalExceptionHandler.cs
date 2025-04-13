using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ShadowNetBackend.Exceptions;

namespace ShadowNetBackend.Middleware;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "An error occurred");

        var problemDetails = new ProblemDetails
        {
            Instance = httpContext.Request.Path,
            Title = "An unexpected error occurred."
        };

        if (exception is BaseException baseException)
        {
            problemDetails.Status = baseException.StatusCode;
            problemDetails.Detail = exception.Message;

            if (baseException is Exceptions.ValidationException validationException)
            {
                problemDetails.Extensions["errors"] = validationException.Errors;
            }
        }
        else
        {
            problemDetails.Status = StatusCodes.Status500InternalServerError;
            problemDetails.Detail = "An internal server error occurred.";
        }

        httpContext.Response.StatusCode = problemDetails.Status.Value;
        httpContext.Response.ContentType = "application/json";
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true; // Indicates the exception was handled
    }
}
