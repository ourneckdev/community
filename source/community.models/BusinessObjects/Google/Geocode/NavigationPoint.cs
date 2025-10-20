using System.Text.Json.Serialization;

namespace community.models.BusinessObjects.Google.Geocode;

/// <summary>
///  contains a list of points that are useful for navigating to the place.
/// </summary>
public class NavigationPoint
{
    /// <summary>
    /// contains the latitude, longitude value of the navigation point.
    /// </summary>
    [JsonPropertyName("location")]
    public Location? Location { get; set; }

    /// <summary>
    /// list of travel modes that the navigation point is not accessible from
    /// </summary>
    [JsonPropertyName("restricted_travel_mode")]
    public string? RestrictedTravelMode { get; set; }
}