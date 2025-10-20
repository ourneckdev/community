using System.Diagnostics.CodeAnalysis;
using community.middleware.Configurations;
using community.middleware.SwaggerFilters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace community.middleware.ServiceCollectionExtensions;

/// <summary>
///     Configures swashbuckle for API documentation.  Accepts a configuration model for customization of the output.
///     Configures the API sample to accept a Bearer token for use with authorizing requests.
/// </summary>
[ExcludeFromCodeCoverage]
public static class ConfigureSwaggerSwashbuckle
{
    /// <summary>
    ///     Adds swashbuckle support to the APIs for documentation.
    /// </summary>
    /// <param name="services">The service collection being extended.</param>
    /// <param name="swaggerGenOptions">The configuration options to assign to swashbuckle, configurable per API.</param>
    public static void AddSwaggerSwashbuckle(this IServiceCollection services, SwaggerConfiguration swaggerGenOptions)
    {
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(opts =>
        {
            opts.EnableAnnotations();
            opts.SchemaFilter<SwaggerRequireNonNullablePropertiesSchemaFilter>();
            opts.OperationFilter<CorrelationIdOperationFilter>();
            opts.OperationFilter<SwaggerExcludeIgnoredPropertiesGetRequestsFilter>();
            opts.SwaggerDoc(swaggerGenOptions.Version
                , new OpenApiInfo
                {
                    Version = swaggerGenOptions.Version,
                    Title = swaggerGenOptions.Title,
                    Description = swaggerGenOptions.Description
                });
            
            if (swaggerGenOptions.ShouldDisplayAuthorization)
                opts.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer"
                });

            opts.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Id = "Bearer",
                            Type = ReferenceType.SecurityScheme
                        }
                    },
                    []
                }
            });

            opts.IgnoreObsoleteActions();
            opts.IgnoreObsoleteProperties();

            foreach (var file in swaggerGenOptions.DocumentationFiles) opts.IncludeXmlComments(file);
        });
    }
}