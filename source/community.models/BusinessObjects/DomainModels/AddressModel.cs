namespace community.models.BusinessObjects.DomainModels;

/// <summary>
///     Defines a DTO object to be used for manipulating data prior to saving.
/// </summary>
public abstract class AddressModel : BaseCommunityModel
{
    /// <summary>
    ///     Optional lot number
    /// </summary>
    public string? LotNumber { get; set; }

    /// <summary>
    ///     Gets or sets the first line of the street address
    /// </summary>
    public string AddressLine1 { get; set; } = "";

    /// <summary>
    ///     Gets or sets an optional second line of the street address.
    /// </summary>
    public string? AddressLine2 { get; set; }

    /// <summary>
    ///     Gets or sets an optional third line of the street address
    /// </summary>
    public string? AddressLine3 { get; set; }

    /// <summary>
    ///     Gets or sets the address city.
    /// </summary>
    public string City { get; set; } = "";

    /// <summary>
    ///     Gets or sets the state code for the address.
    /// </summary>
    public string StateCode { get; set; } = "";

    /// <summary>
    ///     gets or sets the postal code of the address.
    /// </summary>
    public string PostalCode { get; set; } = "";

    /// <summary>
    ///     Gets or sets the optional Id of the county.
    /// </summary>
    public string? CountyCode { get; set; }

    /// <summary>
    ///     Gets or sets the country code for the address
    /// </summary>
    public string CountryCode { get; set; } = "";

    /// <summary>
    ///     Gets or sets the optional Longitudinal coordinate of the address.
    /// </summary>
    public decimal? Longitude { get; set; }

    /// <summary>
    ///     Gets or sets the optional Latitudinal coordinate of the address.
    /// </summary>
    public decimal? Latitude { get; set; }

    /// <summary>
    ///     Gets or sets a string representation of the timezone.
    /// </summary>
    public string TimeZone { get; set; } = null!;

    /// <summary>
    ///     Get or sets the standard timezone offset
    /// </summary>
    public TimeSpan TimeZoneOffset { get; set; }

    /// <summary>
    /// </summary>
    public string? PlaceId { get; set; }
}