using System.Diagnostics;

namespace ShadowNetBackend.Behaviors;

public class LogginBehavior<TRequest, TResponse>(ILogger<LogginBehavior<TRequest, TResponse>> logger) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull, IRequest<TResponse>
    where TResponse : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        logger.LogInformation("[START] Handler request={Request} - Response={Response} - RequestData={RequestData}",
            typeof(TRequest).Name, typeof(TResponse).Name, request);

        var timer = new Stopwatch();
        timer.Start();

        var response = await next();

        timer.Stop();

        var timeTaken = timer.Elapsed;
        if (timeTaken.Seconds > 4)
            logger.LogWarning("[PERFORMANCE] The request {Request} took {TimeTaken} seconds", typeof(TRequest).Name, timeTaken.Seconds);

        logger.LogInformation("[END] Handler {Request} with {Response}", typeof(TRequest).Name, typeof(TResponse).Name);
        return response;
    }
}
