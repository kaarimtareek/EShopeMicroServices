using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Behaviors;

public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("[START] Handling {RequestName} with request {@Request} and Response={@ResponseName}",
            typeof(TRequest).Name, request, typeof(TResponse).Name);
        var timer = new Stopwatch();
        timer.Start();
        var response = await next();
        timer.Stop();
        var timeTaken = timer.Elapsed;
        var elapsedTime =timer.ElapsedMilliseconds;
        if (timeTaken.TotalSeconds > 3)
        {
            logger.LogWarning(
                "[PERFORMANCE] Handling Request {RequestName} with Response {ResponseName} took {TimeTaken} seconds",
                typeof(TRequest).Name, typeof(TResponse).Name,
                timeTaken.TotalSeconds);
        }

        logger.LogInformation("[END] Handling {RequestName} with request {@Request} and Response={@ResponseName} took {TimeTaken}ms",
            typeof(TRequest).Name, request, typeof(TResponse).Name, elapsedTime);
        return response;
    }
}