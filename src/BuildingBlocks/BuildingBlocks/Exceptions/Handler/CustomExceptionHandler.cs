using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Exceptions.Handler;

public class CustomExceptionHandler(ILogger<CustomExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        logger.LogError(exception, "An unhandled exception occurred while processing the request.");
        (string Detail, string Title, int StatusCode) details = exception switch
        {
            InternalServerException => (exception.Message, "Internal Server Error",
                StatusCodes.Status500InternalServerError),
            BadRequestException => (exception.Message, "Bad Request", StatusCodes.Status400BadRequest),
            NotFoundException => (exception.Message, "Not Found", StatusCodes.Status404NotFound),
            ValidationException => (exception.Message, "Validation Error",
                StatusCodes.Status400BadRequest),
            _ => ("An unexpected error occurred.", "Error", StatusCodes.Status500InternalServerError),
        };
        var problemDetails = new ProblemDetails
        {
            Title = details.Title,
            Detail = details.Detail,
            Status = details.StatusCode,
            Instance = httpContext.Request.Path
        };
        problemDetails.Extensions.TryAdd("traceId", httpContext.TraceIdentifier);
        if (exception is ValidationException validationException)
            problemDetails.Extensions.TryAdd("validationErrors", validationException.Errors);
        httpContext.Response.StatusCode = details.StatusCode;
        httpContext.Response.ContentType = "application/problem+json";
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        return true;
    }
}