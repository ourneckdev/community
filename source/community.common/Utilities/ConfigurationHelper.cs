using Microsoft.Extensions.Configuration;

namespace community.common.Utilities;

/// <summary>
/// Helper class for accessing configuration from static classes
/// </summary>
public static class ConfigurationHelper
{
    /// <summary>
    /// 
    /// </summary>
    public static IConfiguration Configuration { get; private set; } = null!;

    /// <summary>
    /// Initializes the helper with the configuration at starutp
    /// </summary>
    /// <param name="configuration"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public static void Initialize(IConfiguration configuration)
    {
        Configuration =  configuration ?? throw new ArgumentNullException(nameof(configuration));
    }
}