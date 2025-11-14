using community.common.BaseClasses;
using community.models.Responses.Lookups;
using community.providers.lookups.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace community.lookups.api.Controllers;

/// <summary>
///     Exposes endpoints returning lookup type data
/// </summary>
/// <param name="provider">The provider logic required for retrieving the appropriate types</param>
[AllowAnonymous]
[ApiController]
[Route("lookups")]
public class LookupsController(ILookupProvider provider) : BaseController
{
    /// <summary>
    ///     Retrieves the available address types including custom types per community
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("addresstypes")]
    [ProducesResponseType(typeof(IEnumerable<AddressTypeResponse>), 200)]
    public async ValueTask<IActionResult> ListAddressTypesAsync()
    {
        return Ok(await provider.ListAddressTypesAsync());
    }

    /// <summary>
    ///     Retrieves the available contact methods including any custom methods per community
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("contactmethods")]
    [ProducesResponseType(typeof(IEnumerable<ContactMethodResponse>), 200)]
    public async ValueTask<IActionResult> ListContactMethodAsync()
    {
        return Ok(await provider.ListContactMethodsAsync());
    }

    /// <summary>
    ///     Retrieves the available report types that can be made
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("reporttypes")]
    [ProducesResponseType(typeof(IEnumerable<ReportTypeResponse>), 200)]
    public async ValueTask<IActionResult> ListReportTypeAsync()
    {
        return Ok(await provider.ListReportTypesAsync());
    }

    /// <summary>
    ///     Retrieves the available user types for security purposes
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("usertypes")]
    [ProducesResponseType(typeof(IEnumerable<UserTypeResponse>), 200)]
    public async ValueTask<IActionResult> ListUserTypeAsync()
    {
        return Ok(await provider.ListUserTypesAsync());
    }

    /// <summary>
    ///     Retrieves the available user types for security purposes
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("parcelsizeunits")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<ParcelSizeUnitResponse>), 200)]
    public async ValueTask<IActionResult> ListParcelSizeUnitsAsync()
    {
        return Ok(await provider.ListParcelSizeUnitsAsync());
    }
}