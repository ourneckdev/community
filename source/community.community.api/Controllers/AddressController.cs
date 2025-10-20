using community.common.BaseClasses;
using community.models.Requests.Addresses;
using community.providers.community.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace community.community.api.Controllers;

/// <summary>
/// 
/// </summary>
[Authorize]
[ApiController]
[Route("address")]
public class AddressController(IAddressProvider addressProvider) : BaseController
{
    /// <summary>
    /// Saves a user address
    /// </summary>
    /// <param name="request"></param>
    /// <param name="token"></param>
    /// <returns>The ID of the saved record and a boolean indicating success.</returns>
    [HttpPost, Route("user")]
    [ProducesResponseType(typeof((Guid AddressId, bool Success)), StatusCodes.Status200OK)]
    public async Task<IActionResult> AddUserAddressAsync(AddAddressRequest request, CancellationToken token = default)
    {
        return Ok(await addressProvider.SaveUserAddressAsync(request, token));
    }

    /// <summary>
    /// Saves a comunity address
    /// </summary>
    /// <param name="request"></param>
    /// <param name="token"></param>
    /// <returns>The ID of the saved record and a boolean indicating success.</returns>
    [HttpPost, Route("community")]
    [ProducesResponseType(typeof((Guid AddressId, bool Success)), StatusCodes.Status200OK)]
    public async Task<IActionResult> AddCommunityAddressAsync(AddAddressRequest request,
        CancellationToken token = default)
    {
        return Ok(await addressProvider.SaveCommunityAddressAsync(request, token));
    }


    /// <summary>
    /// Saves a user address
    /// </summary>
    /// <param name="request"></param>
    /// <param name="token"></param>
    /// <returns>The ID of the saved record and a boolean indicating success.</returns>
    [HttpPost, Route("user/update")]
    [ProducesResponseType(typeof((Guid AddressId, bool Success)), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateUserAddressAsync(EditAddressRequest request,
        CancellationToken token = default)
    {
        return Ok(await addressProvider.SaveUserAddressAsync(request, token));
    }

    /// <summary>
    /// Saves a comunity address
    /// </summary>
    /// <param name="request"></param>
    /// <param name="token"></param>
    /// <returns>The ID of the saved record and a boolean indicating success.</returns>
    [HttpPost, Route("community/update")]
    [ProducesResponseType(typeof((Guid AddressId, bool Success)), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateCommunityAddressAsync(EditAddressRequest request,
        CancellationToken token = default)
    {
        return Ok(await addressProvider.SaveCommunityAddressAsync(request, token));
    }

    /// <summary>
    /// Retrieves a user's address by the address id.
    /// </summary>
    /// <param name="addressId"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpGet, Route("user/{addressId}")]
    public async Task<IActionResult> GetUserAddressAsync(Guid addressId, CancellationToken token = default)
    {
        return Ok(await addressProvider.GetUserAddressAsync(addressId, token));
    }
}