using System.Diagnostics;
using community.common.Definitions;
using Microsoft.AspNetCore.Http;

namespace community.common.BaseClasses;

/// <summary>
///     Abstract functionality to extend to all repository classes
/// </summary>
public abstract class BaseRepository(IHttpContextAccessor contextAccessor)
{
    /// <summary>
    ///     Retrieves the correlation key from the http context.
    /// </summary>
    protected Guid CorrelationId
    {
        get
        {
            Guid.TryParse($"{contextAccessor.HttpContext?.Items[Strings.Header_CorrelationId]}",
                out var CorrelationId);
            return CorrelationId;
        }
    }

    /// <summary>
    /// Retrieves the current community a user is logged into from the JWT user.
    /// </summary>
    protected Guid? CurrentCommunityId
    {
        get
        {
            Guid.TryParse(contextAccessor.HttpContext?.User.Claims.FirstOrDefault(claim => claim.Type == CommunityClaims.CurrentCommunityId)?.Value, out var communityId);
            return communityId;
        }
    }

    /// <summary>
    /// Retrieves the current user id from the JWT, if authenticated.
    /// </summary>
    protected Guid? UserId
    {
        get
        {
            Guid.TryParse(contextAccessor.HttpContext?.User.Claims.FirstOrDefault(claim => claim.Type == CommunityClaims.UserId)?.Value, out var userId);
            return userId;
        }
    }

    /// <summary>
    ///     Asynchronous execution counter
    /// </summary>
    /// <param name="action">The underlying operation to perform</param>
    /// <returns>The execution timespan, as well as the results of the underlying operation</returns>
    protected async Task<(TimeSpan Elapsed, T Results)> MeasureExecutionAsync<T>(Func<Task<T>> action)
    {
        var stopwatch = Stopwatch.StartNew();
        var results = await action();
        stopwatch.Stop();
        return (stopwatch.Elapsed, results);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="methodName"></param>
    /// <param name="executionTimeInMilliseconds"></param>
    /// <returns></returns>
    protected string PrepareInformationLog(string methodName, long executionTimeInMilliseconds)
    {
        return $@"""{methodName}"": {{\n" +
               @"\t ""Status"": ""success"",\n" +
               $@"\t""Elapsed Time (ms)"": {executionTimeInMilliseconds},\n" +
               $@"\t""Execution Time"": {DateTime.UtcNow},\n" +
               "}}";
    }
}