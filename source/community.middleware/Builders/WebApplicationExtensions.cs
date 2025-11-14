using Carter;
using community.middleware.Configurations;
using community.middleware.Handlers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi;
using SimpleInjector;

namespace community.middleware.Builders;

/// <summary>
///     Extension method for reducing duplicity in API startups
/// </summary>
public static class WebApplicationExtensions
{
    /// <summary>
    /// </summary>
    /// <param name="app">The built WebApplication</param>
    /// <param name="container">The SimpleInjector container.</param>
    /// <param name="configuration">The Swagger configuration</param>
    /// <param name="useCarter"></param>
    public static void RegisterApplication(this WebApplication app, Container container,
        SwaggerConfiguration configuration, bool useCarter = false)
    {
        app.UseExceptionHandler();
        app.Services.UseSimpleInjector(container);

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsProduction()) app.MapOpenApi();

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseMiddleware<CorrelationIdHandler>();

        app.UseSwagger(options => options.OpenApiVersion = OpenApiSpecVersion.OpenApi2_0);
        app.UseSwaggerUI(config =>
        {
            config.SwaggerEndpoint("/swagger/v1/swagger.json", $"{configuration.Title} {configuration.Version}");
            config.InjectStylesheet("/swagger-ui/custom.css");
        });

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseHsts();

        if (!useCarter)
            app.MapControllers();
        else
            app.MapCarter();

        container.Verify();
    }
}