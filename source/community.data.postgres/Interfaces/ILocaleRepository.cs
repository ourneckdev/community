using community.common.Interfaces;
using community.data.entities.Locales;
using TimeZone = community.data.entities.Locales.TimeZone;

namespace community.data.postgres.Interfaces;

/// <summary>
///     Retrieves locale specific lookup information, countries, states, counties and timezones
/// </summary>
public interface ILocaleRepository : IRepository
{
    /// <summary>
    ///     Gets a list of all the countries, sorted alphabetically, with the UnitedStates at the top of the list.
    /// </summary>
    /// <returns>A hydrated collection of all available countries in the database.</returns>
    Task<IEnumerable<Country>> ListCountriesAsync();

    /// <summary>
    ///     Gets a collection of all state/provinces by country.
    /// </summary>
    /// <param name="countryCode">The three digit country code to retrieve the list of states for.</param>
    /// <returns>A hydrated collection of all available states by country.</returns>
    Task<IEnumerable<State>> ListStatesAsync(string countryCode);

    /// <summary>
    ///     Retrieves a list of all counties/regions by state.
    /// </summary>
    /// <param name="countryCode"></param>
    /// <param name="stateCode"></param>
    /// <returns></returns>
    Task<IEnumerable<County>> ListCountiesAsync(string countryCode, string stateCode);

    /// <summary>
    ///     Lists the PG available timezones for a specified country
    /// </summary>
    /// <param name="countryCode"></param>
    /// <returns></returns>
    Task<IEnumerable<TimeZone>> ListTimeZonesAsync(string countryCode);

    /// <summary>
    ///     Gets the timezone details by name
    /// </summary>
    /// <param name="countryCode"></param>
    /// <param name="name"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>A</returns>
    Task<TimeZone?> GetTimeZoneAsync(string countryCode, string name,
        CancellationToken cancellationToken = default);
}