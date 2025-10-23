using community.common.BaseClasses;
using community.models.Responses.Lookups;
using community.providers.lookups.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace community.community.api.Controllers;

/// <summary>
///     Exposes endpoints returning lookup type data that has customizable options.  These endpoints require authorization.
/// </summary>
/// <param name="provider">The provider logic required for retrieving the appropriate types</param>
[Authorize]
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
    public async Task<IActionResult> ListAddressTypesAsync()
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
    public async Task<IActionResult> ListContactMethodAsync()
    {
        return Ok(await provider.ListContactMethodsAsync());
    }
}