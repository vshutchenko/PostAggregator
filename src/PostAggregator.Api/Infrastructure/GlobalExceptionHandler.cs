using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PostAggregator.Api.Exceptions;

using System.Net;

namespace PostAggregator.Api.Infrastructure;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "An exception occured {Message}", exception.Message);

        httpContext.Response.ContentType = "application/problem+json";

        var problemDetails = exception switch
        {
            ServiceException serviceException => new ProblemDetails
            {
                Status = serviceException.StatusCode,
                Title = serviceException.DisplayTitle,
                Detail = serviceException.DisplayMessage
            },
            ValidationException validationException => new ValidationProblemDetails
            {
                Status = (int)HttpStatusCode.BadRequest,
                Title = "Validation Error",
                Errors = validationException.Errors.Select(e => new KeyValuePair<string, string[]>(e.PropertyName, new[] { e.ErrorMessage })).ToDictionary(k => k.Key, v => v.Value)
            },
            _ => new ProblemDetails
            {
                Status = (int)HttpStatusCode.InternalServerError,
                Title = "An unexpected error occurred"
            }
        };

        httpContext.Response.StatusCode = problemDetails.Status ?? (int)HttpStatusCode.InternalServerError;

        await httpContext.Response.WriteAsJsonAsync((object)problemDetails, cancellationToken);
        return true;
    }
}
