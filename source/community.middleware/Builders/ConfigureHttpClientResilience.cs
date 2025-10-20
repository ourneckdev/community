using System.Net;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http.Resilience;
using Polly;

namespace community.middleware.Builders;

/// <summary>
/// 
/// </summary>
public static class ConfigureHttpClientResilience
{
    /// <summary>
    /// Sets the default policy for adding resilience to http calls
    /// </summary>
    /// <remarks>
    /// Retry:
    ///   Backoff Type: exponential
    ///   Max Retries: 3
    ///   Use Jitter
    /// Circuit Breaker:
    ///   Sampling Duration: 15 minutes
    ///   Failure Ratio: .2
    ///   Minimum Throughput: 50
    ///   Request timeout and Too Many Requests trigger breaker
    /// </remarks>
    /// <param name="builder">The HttpClientBuilder the configuration should be applied to.</param>
    public static void ConfigureDefaultResilience(this IHttpClientBuilder builder)
    {
        builder
            .AddResilienceHandler("DefaultPolicy", options =>
            {
                options
                    .AddRetry(new HttpRetryStrategyOptions
                    {
                        BackoffType = DelayBackoffType.Exponential,
                        MaxRetryAttempts = 3,
                        UseJitter = true
                    })
                    .AddCircuitBreaker(new HttpCircuitBreakerStrategyOptions
                    {
                        SamplingDuration = TimeSpan.FromMinutes(15),
                        FailureRatio = .2D,
                        MinimumThroughput = 50,
                        ShouldHandle = static args => ValueTask.FromResult(args is
                        {
                            Outcome.Result.StatusCode: HttpStatusCode.RequestTimeout or HttpStatusCode.TooManyRequests
                        })
                    })
                    .AddTimeout(TimeSpan.FromSeconds(10));
            });
    }
}