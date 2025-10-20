using community.common.Definitions;
using Microsoft.AspNetCore.Http;

namespace community.middleware.Handlers;

/// <summary>
///     Middleware for retrieving the correlation key from the header.
/// </summary>
public class CorrelationIdHandler
{
    private readonly RequestDelegate _next;

    /// <summary>
    ///     Constructs the handler, assigning the delegate.
    /// </summary>
    /// <param name="next"></param>
    public CorrelationIdHandler(RequestDelegate next)
    {
        _next = next;
    }

    /// <summary>
    ///     Underlying invocation of appending a correlation key if one does not exist and adding it to the HttpContext.
    /// </summary>
    /// <param name="context">The context of the http request.</param>
    /// <returns></returns>
    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Headers.TryGetValue(Strings.Header_CorrelationId, out var headerValue))
        {
            context.Items.Add(Strings.Header_CorrelationId, headerValue);
        }
        else
        {
            headerValue = Guid.NewGuid().ToString();
            context.Items.Add(Strings.Header_CorrelationId, headerValue);
            context.Request.Headers.Append(Strings.Header_CorrelationId, headerValue);
        }

        context.Response.Headers.Append(Strings.Header_CorrelationId, headerValue);

        await _next(context);
    }
}