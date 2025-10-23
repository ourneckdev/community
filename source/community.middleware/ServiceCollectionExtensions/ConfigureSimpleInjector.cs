using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using SimpleInjector;

namespace community.middleware.ServiceCollectionExtensions;

/// <summary>
///     Sets up  asp.net to add basic SimpleInjector integration.
///     Configures the service collection to use SimpleInjector, registering all controllers and instructing
///     asp.net to let SimpleInjector create those controllers using a Transient lifestyle.
///     Also enables logging and wraps asp.net requests in AsyncScope lifestyle.
/// </summary>
[ExcludeFromCodeCoverage]
public static class ConfigureSimpleInjector
{
    /// <summary>
    ///     Adds simple injector support to the built in container.
    /// </summary>
    /// <param name="services">The service collection being extended.</param>
    /// <param name="container">The container resources are being loaded into.</param>
    /// <param name="useCarter"></param>
    public static void AddSimpleInjector(this IServiceCollection services, Container container, bool useCarter = false)
    {
        services.AddSimpleInjector(container, options =>
            {
                options
                    .AddLogging()
                    .AddAspNetCore()
                    .AddControllerActivation();
            })
            .UseSimpleInjectorAspNetRequestScoping(container);
    }
}