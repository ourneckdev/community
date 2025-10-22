using community.common.BaseClasses;
using community.models.Requests.Authentication;
using community.models.Responses.Authentication;
using community.models.Responses.Base;
using community.providers.auth.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace community.auth.api.Controllers;

/// <summary>
///     Endpoints extended for enabling a user to login.
/// </summary>
/// <param name="authenticationProvider"></param>
[AllowAnonymous]
[ApiController]
[Route("login")]
public class LoginController(IAuthenticationProvider authenticationProvider)
    : BaseController
{
    /// <summary>
    ///     Checks if a user exists.  If found, will generate a login code of length Integers.LoginCodeLength
    ///     to send via the configured notification and returns the login code to the caller.
    /// </summary>
    /// <param name="username">The username to request the login code for</param>
    /// <returns>The generated login code</returns>
    [HttpGet]
    [Route("{username}/code")]
    public async Task<IActionResult> RequestLoginCodeAsync(string username)
    {
        var loginCode = await authenticationProvider.RequestLoginCodeAsync(username);
        return Ok(loginCode);
    }

    /// <summary>
    ///     Allows login using the username and login code.
    /// </summary>
    /// <param name="withCodeRequest"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("")]
    [ProducesResponseType(typeof(LoginResponse),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> LoginAsync([FromBody] LoginWithCodeRequest withCodeRequest)
    {
        return Ok(await authenticationProvider.LoginAsync(withCodeRequest));
    }

    /// <summary>
    ///     Allows login with username and password.
    /// </summary>
    /// <param name="withpasswordRequest"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("withpassword")]
    [ProducesResponseType(typeof(SingleResponse<LoginResponse>),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> WithPasswordAsync([FromBody] LoginWithPasswordRequest withpasswordRequest)
    {
        return Ok(await authenticationProvider.LoginWithPasswordAsync(withpasswordRequest));
    }

    /// <summary>
    /// Endpoint exposing functionality to initiate a password reset process for a user.
    /// </summary>
    /// <param name="forgotPasswordRequest">Post body params necessary for initiating a forgot password flow.</param>
    /// <returns>A boolean indicating success an any relevant messaging.</returns>
    [HttpPost]
    [Route("forgotpassword")]
    public async Task<IActionResult> ForgotPasswordAsync([FromBody] ForgotPasswordRequest forgotPasswordRequest)
    {
        return Ok(await authenticationProvider.ForgotPasswordAsync(forgotPasswordRequest));
    }
}