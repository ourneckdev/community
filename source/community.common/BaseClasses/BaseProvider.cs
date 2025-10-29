using System.Diagnostics;
using System.Security.Claims;
using community.common.Definitions;
using Microsoft.AspNetCore.Http;

namespace community.common.BaseClasses;

/// <summary>
///     Abstract functionality to extend to all provider classes
/// </summary>
public abstract class BaseProvider(IHttpContextAccessor contextAccessor)
{
    private readonly ClaimsPrincipal? _currentUser = contextAccessor.HttpContext?.User;

    /// <summary>
    ///     Retrieves the correlation key from the http context.
    /// </summary>
    protected Guid CorrelationId
    {
        get
        {
            Guid.TryParse($"{contextAccessor.HttpContext?.Items[Strings.Header_CorrelationId]}",
                out var correlationId);
            return correlationId;
        }
    }

    /// <summary>
    ///     Retrieves the current community a user is logged into from the JWT user.
    /// </summary>
    protected Guid? CurrentCommunityId
    {
        get
        {
            Guid.TryParse(
                _currentUser?.Claims.FirstOrDefault(claim => claim.Type == CommunityClaims.CurrentCommunityId)?.Value,
                out var communityId);
            return communityId;
        }
    }

    /// <summary>
    ///     Retrieves the current user id from the JWT, if authenticated.
    /// </summary>
    protected Guid? UserId
    {
        get
        {
            Guid.TryParse(_currentUser?.Claims.FirstOrDefault(claim => claim.Type == CommunityClaims.UserId)?.Value,
                out var userId);
            return userId;
        }
    }

    /// <summary>
    ///     Asynchronous execution counter
    /// </summary>
    /// <param name="action">The underlying operation to perform</param>
    /// <returns>The execution timespan, as well as the results of the underlying operation</returns>
    protected async Task<T> MeasureExecutionAsync<T>(Func<Task<T>> action)
        where T : BaseRecord
    {
        var stopwatch = Stopwatch.StartNew();
        var results = await action();
        stopwatch.Stop();
        results.ExecutionMilliseconds = stopwatch.ElapsedMilliseconds;
        results.CorrelationId = CorrelationId;
        return results;
    }

    /// <summary>
    ///     Logs an information log with request metrics.
    /// </summary>
    /// <param name="methodName">The name of the executing method</param>
    /// <param name="executionTimeInMilliseconds">The time it took for the method to execute, in milliseconds.</param>
    /// <returns>formatted log output</returns>
    protected string PrepareInformationLog(string methodName, long executionTimeInMilliseconds)
    {
        return $@"""{methodName}"": {{\n" +
               @"\t ""Status"": ""success"",\n" +
               $@"\t""Elapsed Time (ms)"": {executionTimeInMilliseconds},\n" +
               $@"\t""Execution Time"": {DateTime.UtcNow},\n" +
               $@"\t""Correlation ID"": {CorrelationId},\n" +
               $@"\t""Community ID"": {CurrentCommunityId},\n" +
               $@"\t""User ID"": {UserId},\n" +
               "}}";
    }
}