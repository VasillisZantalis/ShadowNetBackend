using Microsoft.AspNetCore.Mvc;
using ShadowNetBackend.Exceptions;

namespace ShadowNetBackend.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        _logger.LogError(ex, "An error occurred");

        var problemDetails = new ProblemDetails
        {
            Instance = context.Request.Path,
            Title = "An unexpected error occurred."
        };

        if (ex is BaseException baseException)
        {
            problemDetails.Status = baseException.StatusCode;
            problemDetails.Detail = ex.Message;

            if (baseException is ValidationException validationException)
            {
                problemDetails.Extensions["errors"] = validationException.Errors;
            }
        }
        else
        {
            problemDetails.Status = StatusCodes.Status500InternalServerError;
            problemDetails.Detail = "An internal server error occurred.";
        }

        context.Response.StatusCode = problemDetails.Status.Value;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync(problemDetails);
    }
}
