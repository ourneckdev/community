using System.Text.Json.Serialization;

namespace community.models.BusinessObjects.Google.Geocode;

/// <summary>
/// 
/// </summary>
public class Viewport
{
    /// <summary>
    ///     Gets or sets the northeast viewport bias
    /// </summary>
    [JsonPropertyName("northeast")]
    public Location? Northeast { get; set; }

    /// <summary>
    ///     Get or sets the soutwest viewport bias
    /// </summary>
    [JsonPropertyName("southwest")]
    public Location? Southwest { get; set; }
}