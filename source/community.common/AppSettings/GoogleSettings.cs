namespace community.common.AppSettings;

/// <summary>
///     Defines app settings related to google interactions
/// </summary>
public class GoogleSettings
{
    /// <summary>
    ///     Manages the google geocode base url
    /// </summary>
    public required string GeoCodeBaseUrl { get; init; }

    /// <summary>
    ///     Manages the API key from the appsettings
    /// </summary>
    public required string ApiKey { get; init; }
}