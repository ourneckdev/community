using community.middleware.Builders;
using community.providers.common.HttpClients;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace community.middleware.ServiceCollectionExtensions;

/// <summary>
///     Configures the various http clients and their settings
/// </summary>
public static class ConfigureHttpClients
{
    /// <summary>
    ///     Collectively registers the HttpClients within the container
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    public static void AddHttpClients(this IServiceCollection services, IConfiguration configuration)
    {
        // var assemblies = AssemblyExtensions.FindAssemblies<IHttpClient>();
        // foreach (var assembly in assemblies)
        // {
        //     var types = assembly.GetExportedTypes()
        //         .Where(t => t.IsClass && typeof(IHttpClient).IsAssignableFrom(t) && !t.IsAbstract)
        //         .ToList();
        //     foreach (var type in types)
        //     {
        //         foreach (var interf in type.GetInterfaces().Where(i => i != typeof(IHttpClient)))
        //         services.AddHttpClient()
        //     }
        // }
        services
            .AddHttpClient<IGoogleRestClient, GoogleRestClient>(client =>
            {
                client.BaseAddress =
                    new Uri(configuration.GetSection("GoogleSettings").GetValue<string>("GeoCodeBaseUrl") ??
                            "https://localhost/");
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            })
            .ConfigureDefaultResilience();

        services
            .AddHttpClient<ITwilioHttpClient, TwilioHttpClient>(client =>
            {
                client.BaseAddress = new Uri(configuration.GetSection("TwilioSettings").GetValue<string>("BaseUrl") ??
                                             "https://localhost/");
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            })
            .ConfigureDefaultResilience();
    }
}