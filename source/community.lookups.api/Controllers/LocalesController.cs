using community.models.Responses.Locales;
using community.providers.lookups.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace community.lookups.api.Controllers;

/// <summary>
///     Extends endpoints to expose retrieving geographic values for lookups.
/// </summary>
/// <param name="localeProvider"></param>
[AllowAnonymous]
[ApiController]
[Route("locales")]
public class LocalesController(ILocaleProvider localeProvider) : ControllerBase
{
    /// <summary>
    ///     Retrieves the configured countries available for registration
    /// </summary>
    /// <returns>
    ///     An encapsulating object that contains the returned countries, the correlation key defined on the request and
    ///     execution time in milliseconds
    /// </returns>
    [HttpGet]
    [Route("countries")]
    [ProducesResponseType(typeof(IEnumerable<CountryResponse>), StatusCodes.Status200OK)]
    public async ValueTask<IActionResult> ListCountriesAsync()
    {
        return Ok(await localeProvider.ListCountriesAsync());
    }

    /// <summary>
    ///     Retrieves the configured state/provinces for a specific country available for registration
    /// </summary>
    /// <param name="countryCode">The three character country code</param>
    /// <returns>
    ///     An encapsulating object that contains the returned states, the correlation key defined on the request and
    ///     execution time in milliseconds
    /// </returns>
    [HttpGet]
    [Route("countries/{countryCode}/states")]
    [ProducesResponseType(typeof(IEnumerable<StateResponse>), StatusCodes.Status200OK)]
    public async ValueTask<IActionResult> ListStatesAsync(string countryCode)
    {
        return Ok(await localeProvider.ListStatesAsync(countryCode));
    }

    /// <summary>
    ///     Returns a list of counties for a specified country and state.
    /// </summary>
    /// <param name="countryCode">The three character country code</param>
    /// <param name="stateCode">The two character state code the counties are contained within.</param>
    /// <returns>
    ///     An encapsulating object that contains the returned counties, the correlation key defined on the request and
    ///     execution time in milliseconds
    /// </returns>
    [HttpGet]
    [Route("countries/{countryCode}/states/{stateCode}")]
    [ProducesResponseType(typeof(IEnumerable<CountyResponse>), StatusCodes.Status200OK)]
    public async ValueTask<IActionResult> ListCountiesAsync(string countryCode, string stateCode)
    {
        return Ok(await localeProvider.ListCountiesAsync(countryCode, stateCode));
    }

    /// <summary>
    ///     Lists the available timezones for the supplied country.
    /// </summary>
    /// <param name="countryCode"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("countries/{countryCode}/timezones")]
    [ProducesResponseType(typeof(IEnumerable<TimeZoneResponse>), StatusCodes.Status200OK)]
    public async ValueTask<IActionResult> ListTimeZonesAsync(string countryCode)
    {
        return Ok(await localeProvider.ListTimeZonesAsync(countryCode));
    }
}