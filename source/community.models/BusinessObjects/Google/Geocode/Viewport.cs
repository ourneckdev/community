namespace community.models.BusinessObjects.Google.Geocode;

/// <summary>
/// </summary>
public class Viewport
{
    /// <summary>
    ///     Gets or sets the northeast viewport bias
    /// </summary>
    public Location? Northeast { get; init; }

    /// <summary>
    ///     Get or sets the soutwest viewport bias
    /// </summary>
    public Location? Southwest { get; init; }
}