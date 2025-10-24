using community.common.AppSettings;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace community.middleware.Builders;

/// <summary>
///     Adds configurations in appropriate order starting with the appsettings.json
/// </summary>
/// <remarks>Will extend to use SSM when moving to a cloud environment.</remarks>
public static class ConfigureAppSettings
{
    /// <summary>
    ///     Extends the host applicaiton builder to add configurations
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static void AddOrderedConfigurations(this IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((hostingContext, options) =>
        {
            var env = hostingContext.HostingEnvironment;

            options.SetBasePath(Directory.GetCurrentDirectory());
            options.AddJsonFile("appsettings.json", true, true);
            options.AddJsonFile($"appsettings.{env.EnvironmentName}.json", false, true);
        });
    }

    /// <summary>
    ///     Sets IOptions classes to the entire configuration isn't tightly coupled to the app.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    public static void SetOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)));
        services.Configure<GoogleSettings>(configuration.GetSection(nameof(GoogleSettings)));
    }
}