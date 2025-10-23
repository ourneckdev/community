namespace community.common.AppSettings;

/// <summary>
///     Defines app settings related to google interactions
/// </summary>
public class GoogleSettings
{
    /// <summary>
    ///     Manages the google geocode base url
    /// </summary>
    public string GeoCodeBaseUrl { get; set; } = nameof(GeoCodeBaseUrl);

    /// <summary>
    ///     Manages the API key from the appsettings
    /// </summary>
    public string ApiKey { get; set; } = nameof(ApiKey);
}