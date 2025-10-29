using community.common.Interfaces;
using community.models.Requests.Communities;
using community.models.Responses;
using community.models.Responses.Base;

namespace community.providers.community.Interfaces;

/// <summary>
///     Defines the operations that can be performed against Community data.
/// </summary>
public interface ICommunityProvider : IProvider
{
    /// <summary>
    ///     Searches for a community using the supplied criteria and returns any applicable responses.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<MultiResponse<CommunityResponse>> SearchAsync(FindCommunityRequest request);

    /// <summary>
    /// Retrieves a community by id.
    /// </summary>
    /// <param name="id">The ID of the community</param>
    /// <returns>A community response object including Address and Contact information.</returns>
    Task<SingleResponse<CommunityResponse>> GetAsync(Guid id);
}