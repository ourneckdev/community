using System.Text.Json.Serialization;

namespace community.models.BusinessObjects.Google.Geocode;

/// <summary>
/// Defines the address components portion of the response from the GeoCode API
/// </summary>
public class AddressComponent
{
    /// <summary>
    /// Gets or sets the long name
    /// </summary>
    [JsonPropertyName("long_name")]
    public string LongName { get; set; } = null!;
    
    /// <summary>
    /// Gets or sets the short name
    /// </summary>
    [JsonPropertyName("short_name")]
    public string ShortName { get; set; } = null!;

    /// <summary>
    /// Gets or sets the collection of types for the component.
    /// </summary>
    [JsonPropertyName("types")]
    public List<string> Types { get; set; } = null!;
}