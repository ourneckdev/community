using community.common.BaseClasses;
using community.models.Requests.Users;
using community.models.Responses;
using community.models.Responses.Base;
using community.providers.community.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace community.community.api.Controllers;

/// <summary>
/// Exposes endpoints for saving and retrieving user data.
/// </summary>
/// <param name="userProvider"></param>
[ApiController]
[Route("user")]
public class UserController(IUserProvider userProvider) : BaseController
{
    /// <summary>
    /// Retrieves a user's information.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("{id}")]
    [ProducesResponseType(typeof(SingleResponse<UserResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return Ok(await userProvider.GetAsync(id, cancellationToken));
    }

    /// <summary>
    /// Updates an existing user  
    /// </summary>
    /// <param name="request">Optional and required info necessary to update user.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>The id of the newly created user.</returns>
    [HttpPost]
    [Route("add")]
    [ProducesResponseType(typeof(SingleResponse<Guid>), StatusCodes.Status200OK)]
    public async Task<IActionResult> AddAsync(AddUserRequest request, CancellationToken cancellationToken = default)
    {
        return Ok(await userProvider.AddAsync(request, cancellationToken));
    }

    /// <summary>
    /// Updates an existing user  
    /// </summary>
    /// <param name="request">Optional and required info necessary to update user.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>The id of the newly created user.</returns>
    [HttpPost]
    [Route("update")]
    [ProducesResponseType(typeof(SingleResponse<Guid>), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateAsync(UpdateUserRequest request,
        CancellationToken cancellationToken = default)
    {
        return Ok(await userProvider.UpdateAsync(request, cancellationToken));
    }

    /// <summary>
    /// During the registration process, a user will be required to verify they hold either
    /// the mobile number or email address used for their username.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("verifyusername")]
    [ProducesResponseType(typeof(SingleResponse<bool>), StatusCodes.Status200OK)]
    public async Task<IActionResult> VerifyUsernameAsync(VerifyUserNameRequest request,
        CancellationToken cancellationToken = default)
    {
        return Ok(await userProvider.VerifyUserNameAsync(request, cancellationToken));
    }
}