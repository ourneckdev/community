using community.common.Definitions;
using community.common.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace community.middleware.Handlers;

/// <summary>
///     Abstract exception handler for all API enpoints.
/// </summary>
// public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
public class GlobalExceptionHandler : IExceptionHandler
{
    /// <summary>
    ///     Handles the exception conditions.
    /// </summary>
    /// <param name="httpContext">The context of the current request pipeline.</param>
    /// <param name="exception">The exception thrown</param>
    /// <param name="cancellationToken">A cancellation token for the operation.</param>
    /// <returns>true/false</returns>
    /// <exception cref="NotImplementedException">Stand in.</exception>
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        var problemDetails = exception switch
        {
            ValidationException ex => new ValidationProblemDetails(ex.Errors)
            {
                Title = ValidationMessages.ValidationErrors,
                Type = "https://httpstatuses.com/400",
                Status = StatusCodes.Status400BadRequest,
                Detail = ex.Message,
                Instance = httpContext.Request.Path
            },
            NotFoundException ex => new ProblemDetails
            {
                Title = "Entity not found",
                Detail = ex.Message,
                Status = StatusCodes.Status404NotFound,
                Instance = httpContext.Request.Path
            },
            BusinessRuleException ex => new ProblemDetails
            {
                Title = "Constraint violation",
                Type = "https://httpstatuses.com/400",
                Detail = ex.Message,
                Status = StatusCodes.Status400BadRequest,
                Instance = httpContext.Request.Path
            },
            _ => new ProblemDetails
            {
                Title = "Internal Server Error",
                Status = StatusCodes.Status500InternalServerError,
                Detail = exception.Message,
                Instance = httpContext.Request.Path
            }
        };
        httpContext.Response.ContentType = "application/problem+json";
        httpContext.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status400BadRequest;
        var newtonSoftProblemDetails = JsonConvert.SerializeObject(problemDetails);
        await httpContext.Response.WriteAsync(newtonSoftProblemDetails, cancellationToken);

        return true;
    }
}