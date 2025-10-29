namespace community.models.BusinessObjects.Google.Geocode;

/// <summary>
///     contains a list of points that are useful for navigating to the place.
/// </summary>
public class NavigationPoint
{
    /// <summary>
    ///     contains the latitude, longitude value of the navigation point.
    /// </summary>
    public Location? Location { get; init; }

    /// <summary>
    ///     list of travel modes that the navigation point is not accessible from
    /// </summary>
    public string? RestrictedTravelMode { get; init; }
}