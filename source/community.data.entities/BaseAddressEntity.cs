using System.ComponentModel.DataAnnotations;
using community.common.BaseClasses;
using community.data.entities.Locales;
using community.data.entities.Lookups;

namespace community.data.entities;

/// <summary>
///     Represents an address, that can be tied to a user or other entity.
/// </summary>
public abstract class BaseAddressEntity : BaseCommunityEntity
{
    /// <summary>
    ///     Gets or sets the address type by enumeration
    /// </summary>
    public Guid AddressTypeId { get; set; }
    
    /// <summary>
    /// Optional lot number
    /// </summary>
    public string? LotNumber { get; set; }

    /// <summary>
    ///     Gets or sets the first line of the street address
    /// </summary>
    [MaxLength(100)]
    public string AddressLine1 { get; set; } = "";

    /// <summary>
    ///     Gets or sets an optional second line of the street address.
    /// </summary>
    [MaxLength(100)]
    public string? AddressLine2 { get; set; }

    /// <summary>
    ///     Gets or sets an optional third line of the street address
    /// </summary>
    [MaxLength(100)]
    public string? AddressLine3 { get; set; }

    /// <summary>
    ///     Gets or sets the address city.
    /// </summary>
    [MaxLength(100)]
    public string City { get; set; } = "";

    /// <summary>
    ///     Gets or sets the state code for the address.
    /// </summary>
    [Length(2, 2)]
    public string StateCode { get; set; } = "";

    /// <summary>
    ///     gets or sets the postal code of the address.
    /// </summary>
    [MaxLength(20)]
    public string PostalCode { get; set; } = "";

    /// <summary>
    ///     Gets or sets the optional Id of the county.
    /// </summary>
    public string? CountyCode { get; set; }

    /// <summary>
    ///     Gets or sets the country code for the address
    /// </summary>
    [Length(3, 3)]
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
    /// Gets or sets a string representation of the timezone.
    /// </summary>
    public string TimeZone { get; set; } = null!;
    
    /// <summary>
    /// Get or sets the standard timezone offset 
    /// </summary>
    public TimeSpan TimeZoneOffset { get; set; }
    
    /// <summary>
    /// Gets or sets the GoogleSettings GeoCode Place Id
    /// </summary>
    public string? PlaceId { get; set; }


    #region Navigation Properties
    
    /// <summary>
    ///     Navigation property for join conditions for states.
    /// </summary>
    public State State { get; set; } = null!;

    /// <summary>
    ///     Navigation property for join conditions for counties.
    /// </summary>
    public County County { get; set; } = null!;

    /// <summary>
    ///     Navigation property for join conditions for countries.
    /// </summary>
    public Country Country { get; set; } = null!;

    /// <summary>
    ///     Navigation property.
    /// </summary>
    public AddressType? AddressType { get; set; } 

    #endregion
}