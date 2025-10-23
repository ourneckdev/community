using community.models.Abstract;

namespace community.models.Requests.Addresses;

/// <summary>
/// </summary>
/// <param name="CommunityId"></param>
/// <param name="AddressTypeId"></param>
/// <param name="LotNumber"></param>
/// <param name="AddressLine1"></param>
/// <param name="AddressLine2"></param>
/// <param name="AddressLine3"></param>
/// <param name="City"></param>
/// <param name="StateCode"></param>
/// <param name="TimeZone"></param>
/// <param name="PostalCode"></param>
/// <param name="CountyCode"></param>
/// <param name="CountryCode"></param>
public record AddAddressRequest(
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
    : BaseAddressRequest(CommunityId, AddressTypeId, LotNumber, AddressLine1, AddressLine2, AddressLine3, City,
        StateCode, TimeZone, PostalCode, CountyCode, CountryCode)
{
    /// <summary>
    ///     Initializes the record with a user id
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
    public AddAddressRequest(
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