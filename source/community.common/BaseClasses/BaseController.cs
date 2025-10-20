using System.Diagnostics;
using community.common.Definitions;
using Microsoft.AspNetCore.Mvc;

namespace community.common.BaseClasses;

/// <summary>
///     Abstract functionality to expose to API endpoints.
/// </summary>
public abstract class BaseController : ControllerBase
{
    /// <summary>
    ///     Retrieves the correlation key from the http context.
    /// </summary>
    protected Guid CorrelationId
    {
        get
        {
            Guid.TryParse($"{HttpContext.Items[Strings.Header_CorrelationId]}", out var correlationId);
            return correlationId;
        }
    }

    /// <summary>
    ///     Asynchronous execution counter
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    protected async Task<(TimeSpan Elapsed, T Results)> MeasureExecutionAsync<T>(Func<Task<T>> action)
    {
        var stopwatch = Stopwatch.StartNew();
        var results = await action();
        stopwatch.Stop();
        return (stopwatch.Elapsed, results);
    }
}