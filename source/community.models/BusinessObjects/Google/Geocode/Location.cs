using System.Text.Json.Serialization;

namespace community.models.BusinessObjects.Google.Geocode;

/// <summary>
///     Represents the location portion of the Geometry response.
/// </summary>
public class Location
{
    /// <summary>
    ///     Gets or sets the latitude
    /// </summary>
    [JsonPropertyName("latitude")]
    public decimal Latitude { get; set; }

    /// <summary>
    ///     Gets or sets the longitude
    /// </summary>
    [JsonPropertyName("longitude")]
    public decimal Longitude { get; set; }

    /// <summary>
    ///     Gets or sets the latitude
    /// </summary>
    [JsonPropertyName("lat")]
    public decimal Lat { get; set; }

    /// <summary>
    ///     Gets or sets the longitude
    /// </summary>
    [JsonPropertyName("lng")]
    public decimal Lng { get; set; }
}