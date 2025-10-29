namespace community.models.BusinessObjects.Google.Geocode;

/// <summary>
///     Defines the address components portion of the response from the GeoCode API
/// </summary>
public class AddressComponent
{
    /// <summary>
    ///     Gets or sets the long name
    /// </summary>
    public required string LongName { get; init; }

    /// <summary>
    ///     Gets or sets the short name
    /// </summary>
    public required string ShortName { get; init; }

    /// <summary>
    ///     Gets or sets the collection of types for the component.
    /// </summary>
    public required List<string> Types { get; init; }
}