using community.common.BaseClasses;
using community.models.Requests.Communities;
using community.models.Responses;
using community.models.Responses.Base;
using community.providers.community.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace community.community.api.Controllers;

/// <summary>
///     Hosts endpoints related to manipulating community data.
/// </summary>
[Authorize]
[ApiController, Route("community")]
public class CommunityController(ICommunityProvider communityProvider) : BaseController
{
    /// <summary>
    /// Executes a search using supplied parameters
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet, Route("search")]
    [ProducesResponseType(typeof(MultiResponse<CommunityResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> FindCommunityAsync([FromQuery] FindCommunityRequest request)
    {
        return Ok(await communityProvider.SearchAsync(request));
    }

    /// <summary>
    /// Retrieves a community by id. 
    /// </summary>
    /// <param name="id">The ID of the community to retrieve</param>
    /// <returns></returns>
    [HttpGet, Route("{id:guid}")]
    [ProducesResponseType(typeof(SingleResponse<CommunityResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAsync(Guid id)
    {
        return Ok(await communityProvider.GetAsync(id));
    }

}