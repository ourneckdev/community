using community.common.BaseClasses;
using community.providers.auth.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace community.auth.api.Controllers;

/// <summary>
/// Extends endpoints for token authorization
/// </summary>
/// <param name="authProvider"></param>
[ApiController]
[Route("token")]
public class TokenController(IAuthenticationProvider authProvider) : BaseController
{
    /// <summary>
    /// Generates a verifier and challenge for validating user auth flow
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [AllowAnonymous]
    [Route("pkce")]
    public IActionResult GeneratePkce()
    {
        return Ok(authProvider.GenerateProof());
    }
}