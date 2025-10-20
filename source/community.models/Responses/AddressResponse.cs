using System.Text.Json.Serialization;
using community.data.entities;
using community.models.Responses.Base;

namespace community.models.Responses;

/// <summary>
///     Represents the response value to be returned via an API call for address related date
/// </summary>
public abstract class AddressResponse : BaseCommunityResponse
{
    #region Properties

    /// Gets or sets a common name to reference the record by.
    public string? LotNumber { get; set; } = "";

    /// <summary>
    ///     Gets or sets the AddressType
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Guid? AddressTypeId { get; set; }

    /// <summary>
    ///     Gets or sets the name of the address type
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? AddressType { get; set; }

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
    ///     Gets or sets the postal code.
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
    ///     Get or sets the timezone of the address
    /// </summary>
    public string TimeZone { get; set; } = null!;

    /// <summary>
    ///     Gets or sets the utc offset of the tiemzone.
    /// </summary>
    public TimeSpan TimeZoneOffset { get; set; }

    /// <summary>
    /// </summary>
    public string? PlaceId { get; set; }

    #endregion
    
    /// <summary>
    /// </summary>
    /// <param name="address"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    protected static T MapValues<T>(BaseAddressEntity address) where T : AddressResponse, new()
    {
        var response = BaseCommunityResponse.MapValues<T>(address);
        response.Id = address.Id;
        response.CommunityId = address.CommunityId;
        response.AddressTypeId = address.AddressTypeId;
        response.AddressType = address.AddressType?.Name;
        response.LotNumber = address.LotNumber;
        response.AddressLine1 = address.AddressLine1;
        response.AddressLine2 = address.AddressLine2;
        response.AddressLine3 = address.AddressLine3;
        response.City = address.City;
        response.StateCode = address.StateCode;
        response.PostalCode = address.PostalCode;
        response.CountyCode = address.CountyCode;
        response.CountryCode = address.CountryCode;
        response.Longitude = address.Longitude;
        response.Latitude = address.Latitude;
        response.TimeZone = address.TimeZone;
        response.TimeZoneOffset = address.TimeZoneOffset;
        response.PlaceId = address.PlaceId;
        return response;
    }
}