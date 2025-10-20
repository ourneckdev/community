using System.Text.Json.Serialization;

namespace community.models.BusinessObjects.Google.Geocode;

/// <summary>
/// </summary>
public class Result
{
    /// <summary>
    ///     an array containing the separate components applicable to this address.
    /// </summary>
    /// <remarks>
    ///     Each address component typically contains the following fields:
    ///     types[] is an array indicating the type of the address component.
    ///     long_name is the full text description or name of the address component as returned by the Geocoder.
    ///     short_name is an abbreviated textual name for the address component, if available. For example, an address
    ///     component for the state of Alaska may have a long_name of "Alaska" and a short_name of "AK" using the 2-letter
    ///     postal abbreviation.
    /// </remarks>
    [JsonPropertyName("address_components")]
    public List<AddressComponent> AddressComponents { get; set; } = null!;

    /// <summary>
    ///     a string containing the human-readable address of this location.
    /// </summary>
    [JsonPropertyName("formatted_address")]
    public string? FormattedAddress { get; set; }

    /// <summary>
    ///     Contains the geometric data from the response
    /// </summary>
    [JsonPropertyName("geometry")]
    public Geometry? Geometry { get; set; }

    /// <summary>
    ///     contains a list of points that are useful for navigating to the place
    /// </summary>
    [JsonPropertyName("navigation_points")]
    public List<NavigationPoint>? NavigationPoints { get; set; }

    /// <summary>
    ///     is a unique identifier that can be used with other GoogleSettings APIs
    /// </summary>
    [JsonPropertyName("place_id")]
    public string? PlaceId { get; set; }

    /// <summary>
    ///     This array contains a set of zero or more tags identifying the type of feature returned in the result.
    /// </summary>
    /// <remarks>
    ///     For example, a geocode of "Chicago" returns "locality" which indicates that "Chicago" is a city, and also
    ///     returns "political" which indicates it is a political entity. Components might have an empty types array when there
    ///     are no known types for that address component.
    /// </remarks>
    [JsonPropertyName("types")]
    public List<string>? Types { get; set; }

    /// <summary>
    ///     Retrieves the long name from an address component by type.
    /// </summary>
    /// <param name="type">The GoogleSettings AddressModel Component Type to return the long </param>
    /// <returns>The response value retrieved from the http call.</returns>
    public AddressComponent? GetComponent(ComponentType type)
    {
        return AddressComponents.FirstOrDefault(component => component.Types.Contains(type.ToString()));
    }

    /// <summary>
    ///     Returns first line of the street address
    /// </summary>
    /// <returns></returns>
    public string GetStreetAddress() => $"{GetComponent(ComponentType.street_number)?.ShortName ?? ""} {GetComponent(ComponentType.route)?.ShortName ?? ""}";
    
    /// <summary>
    /// gets concatenated postal code 
    /// </summary>
    /// <returns></returns>
    public string GetPostalCode() => !string.IsNullOrWhiteSpace(GetComponent(ComponentType.postal_code_suffix)?.ShortName)
        ? $"{GetComponent(ComponentType.postal_code)?.ShortName ?? ""}-{GetComponent(ComponentType.postal_code_suffix)?.ShortName ?? ""}"
        : $"{GetComponent(ComponentType.postal_code)?.ShortName ?? ""}";
    
    /// <summary>
    /// Retrieves Longitude and Latitude from the results.
    /// </summary>
    /// <returns></returns>
    public (decimal? Longitude, decimal? Latitude) GetLatLong() => (Geometry?.Location?.Lng, Geometry?.Location?.Lat); 
    
}