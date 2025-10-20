using System.Text.Json.Serialization;

namespace community.models.BusinessObjects.Google.Geocode;

/// <summary>
/// Encapsulates the returned data from GoogleSettings's Geocoding API
/// </summary>
public class Response
{
    /// <summary>
    /// Gets the collection of results returned.
    /// </summary>
    [JsonPropertyName("results")]
    public List<Result> Results { get; set; } = null!;
    
    /// <summary>
    /// Gets the returned status of the API call.
    /// </summary>
    [JsonPropertyName("status")]
    public string?Status { get; set; }
}