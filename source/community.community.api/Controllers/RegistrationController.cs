using community.common.BaseClasses;
using community.models.Requests.Registration;
using community.models.Responses.Authentication;
using community.models.Responses.Base;
using community.providers.community.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace community.community.api.Controllers;

/// <summary>
///  Hosts endpoints responsible for signing new users up
/// </summary>
[AllowAnonymous]
[ApiController]
[Route("register")]
public class RegistrationController(IRegistrationProvider registrationProvider) : BaseController
{
    /// <summary>
    /// Initial setup for a community who wishes to setup the app for their members.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("community")]
    [ProducesResponseType(typeof(SingleResponse<LoginResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RegisterCommunityAsync(RegisterCommunityRequest request)
    {
        return Ok(await registrationProvider.RegisterCommunityAsync(request));
    }
}