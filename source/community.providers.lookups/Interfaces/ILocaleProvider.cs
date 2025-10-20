using community.common.Interfaces;
using community.models.Responses.Base;
using community.models.Responses.Locales;

namespace community.providers.lookups.Interfaces;


/// <summary>
/// Defines the available methods for retrieving geographic lookup data
/// </summary>
public interface ILocaleProvider : IProvider
{
    /// <summary>
    /// List the countries defined.
    /// </summary>
    /// <returns></returns>
    ValueTask<LookupResponse<CountryResponse>> ListCountriesAsync();

    /// <summary>
    /// List the state by country code
    /// </summary>
    /// <param name="countryCode"></param>
    /// <returns></returns>
    ValueTask<LookupResponse<StateResponse>> ListStatesAsync(string countryCode);
    
    /// <summary>
    /// List the counties by country and state code
    /// </summary>
    /// <param name="countryCode"></param>
    /// <param name="stateCode"></param>
    /// <returns></returns>
    ValueTask<LookupResponse<CountyResponse>> ListCountiesAsync(string countryCode, string stateCode);

    /// <summary>
    /// List the available timezones by country code
    /// </summary>
    /// <param name="countryCode"></param>
    /// <returns></returns>
    ValueTask<LookupResponse<TimeZoneResponse>> ListTimeZonesAsync(string countryCode);
}