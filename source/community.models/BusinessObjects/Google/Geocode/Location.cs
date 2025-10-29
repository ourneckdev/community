namespace community.models.BusinessObjects.Google.Geocode;

/// <summary>
///     Represents the location portion of the Geometry response.
/// </summary>
public class Location
{
    /// <summary>
    ///     Gets or sets the latitude
    /// </summary>
    public decimal Latitude { get; init; }

    /// <summary>
    ///     Gets or sets the longitude
    /// </summary>
    public decimal Longitude { get; init; }

    /// <summary>
    ///     Gets or sets the latitude
    /// </summary>
    public decimal Lat { get; init; }

    /// <summary>
    ///     Gets or sets the longitude
    /// </summary>
    public decimal Lng { get; init; }
}