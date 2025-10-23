using System.Text.Json;
using System.Text.Json.Serialization;
using Carter;
using community.middleware.Builders;
using community.middleware.Configurations;
using community.middleware.Handlers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SimpleInjector;

namespace community.middleware.ServiceCollectionExtensions;

/// <summary>
/// </summary>
public static class ConfigureServices
{
    /// <summary>
    /// </summary>
    /// <param name="services"></param>
    /// <param name="container"></param>
    /// <param name="configuration"></param>
    /// <param name="swaggerConfiguration"></param>
    /// <param name="useCarter"></param>
    public static void RegisterServices(this IServiceCollection services,
        Container container,
        IConfiguration configuration,
        SwaggerConfiguration swaggerConfiguration,
        bool useCarter = false)
    {
        if (useCarter)
        {
            services.AddCarter();
        }
        else
        {
            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.TypeInfoResolverChain.Add(ProblemDetailsSerializerContext.Default);
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });
            ;
            services.AddEndpointsApiExplorer();
        }

        services.SetOptions(configuration);
        services.AddHttpContextAccessor();
        services.AddSimpleInjector(container);
        services.AddJwtBearerAuthentication(configuration);
        services.AddSwaggerSwashbuckle(swaggerConfiguration);
        services.AddMemoryCache();
        services.AddProblemDetails();
        services.AddLogging(options => options.AddConsole());
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddHttpClients(configuration);
    }
}

[JsonSerializable(typeof(ProblemDetails))]
[JsonSerializable(typeof(ValidationProblemDetails))]
[JsonSerializable(typeof(HttpValidationProblemDetails))]
internal sealed partial class ProblemDetailsSerializerContext : JsonSerializerContext;