using System.Text.Json.Serialization;

namespace community.models.BusinessObjects.Google.Geocode;

/// <summary>
/// </summary>
public class Geometry
{
    /// <summary>
    ///     The bounding box of the viewport within which to bias geocode results more prominently. This parameter will only
    ///     influence, not fully restrict, results from the geocoder
    /// </summary>
    [JsonPropertyName("bounds")]
    public Viewport? Bounds { get; set; }

    /// <summary>
    ///     contains the geocoded latitude, longitude value.
    /// </summary>
    [JsonPropertyName("location")]
    public Location? Location { get; set; }

    /// <summary>
    ///     stores additional data about the specified location
    /// </summary>
    /// <remarks>
    ///     "ROOFTOP" indicates that the returned result is a precise geocode for which we have location information accurate
    ///     down to street address precision.
    ///     "RANGE_INTERPOLATED" indicates that the returned result reflects an approximation (usually on a road) interpolated
    ///     between two precise points (such as intersections). Interpolated results are generally returned when rooftop
    ///     geocodes are unavailable for a street address.
    ///     "GEOMETRIC_CENTER" indicates that the returned result is the geometric center of a result such as a polyline (for
    ///     example, a street) or polygon (region).
    ///     "APPROXIMATE" indicates that the returned result is approximate.
    /// </remarks>
    [JsonPropertyName("location_type")]
    public string? LocationType { get; set; } = null!;

    /// <summary>
    ///     contains the recommended viewport for displaying the returned result, specified as two latitude,longitude values
    ///     defining the southwest and northeast corner of the viewport bounding box.
    /// </summary>
    [JsonPropertyName("viewport")]
    public Viewport? Viewport { get; set; }
}