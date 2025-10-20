using community.models.Abstract;

namespace community.models.Requests.Registration;

/// <summary>
///     Defines required and optional parameters expected during the registraiton process
/// </summary>
/// <param name="AddressTypeId">The category of the address</param>
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
public record RegisterAddressRequest(
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
    : BaseAddressRequest(
        AddressTypeId,
        LotNumber,
        AddressLine1,
        AddressLine2,
        AddressLine3,
        City,
        StateCode,
        TimeZone,
        PostalCode,
        CountyCode,
        CountryCode)
{
    /// <summary>
    ///     Outputs the address a string to be passed to google for geocoding
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return $"{AddressLine1} {AddressLine2 ?? " "}{City} {StateCode} {PostalCode} {CountyCode}";
    }
}