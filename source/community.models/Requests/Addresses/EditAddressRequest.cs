using community.models.Abstract;

namespace community.models.Requests.Addresses;

/// <summary>
///     Defines required and optional parameters expected during the registration process
/// </summary>
/// <param name="CommunityId">Gets or sets the community id the address is related to</param>
/// <param name="AddressTypeId">The id of the address record being edited</param>
/// <param name="LotNumber">Gets or sets a friendly name for the address</param>
/// <param name="AddressLine1">Gets or sets the first line of the address</param>
/// <param name="AddressLine2">Optional, gets or sets the 2nd line of the address</param>
/// <param name="AddressLine3">Optional, gets or sets the third line of the address</param>
/// <param name="City">Gets or set the city</param>
/// <param name="StateCode">Gets or set the state</param>
/// <param name="TimeZone"></param>
/// <param name="PostalCode">Gets or set the postalcode</param>
/// <param name="CountyCode">Optional, gets or set the county code</param>
/// <param name="CountryCode">Gets or sets the country code, defaulted to USA</param>
public record EditAddressRequest(
    Guid CommunityId,
    Guid AddressTypeId,
    string? LotNumber,
    string AddressLine1,
    string? AddressLine2,
    string? AddressLine3,
    string City,
    string StateCode,
    string TimeZone,
    string PostalCode,
    string? CountyCode,
    string CountryCode = "USA")
    : BaseAddressRequest(CommunityId, AddressTypeId, LotNumber, AddressLine1, AddressLine2, AddressLine3, City, StateCode, TimeZone, PostalCode, CountyCode, CountryCode)
{
    /// <summary>
    /// Initializes the record with a user id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="communityId"></param>
    /// <param name="userId"></param>
    /// <param name="addressTypeId"></param>
    /// <param name="lotNumber"></param>
    /// <param name="addressLine1"></param>
    /// <param name="addressLine2"></param>
    /// <param name="addressLine3"></param>
    /// <param name="city"></param>
    /// <param name="stateCode"></param>
    /// <param name="timeZone"></param>
    /// <param name="postalCode"></param>
    /// <param name="countyCode"></param>
    /// <param name="countryCode"></param>
    public EditAddressRequest(
        Guid id,
        Guid communityId,
        Guid? userId,
        Guid addressTypeId,
        string? lotNumber,
        string addressLine1,
        string? addressLine2,
        string? addressLine3,
        string city,
        string stateCode,
        string timeZone,
        string postalCode,
        string? countyCode,
        string countryCode = "USA") : this(communityId, addressTypeId, lotNumber, addressLine1, addressLine2,
        addressLine3, city, stateCode, timeZone, postalCode, countyCode, countryCode)
    {
        Id = id;
        UserId = userId;
    }
}