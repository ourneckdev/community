namespace community.models.BusinessObjects.Google.Geocode;

/// <summary>
///     Encapsulates the returned data from GoogleSettings's Geocoding API
/// </summary>
public class Response
{
    /// <summary>
    ///     Gets the collection of results returned.
    /// </summary>
    public required List<Result> Results { get; init; }

    /// <summary>
    ///     Gets the returned status of the API call.
    /// </summary>
    public string? Status { get; init; }
}