using community.common.BaseClasses;
using community.common.Definitions;
using community.common.Exceptions;
using community.data.postgres.Interfaces;
using community.models.Requests.Communities;
using community.models.Responses;
using community.models.Responses.Base;
using community.providers.community.Interfaces;
using Microsoft.AspNetCore.Http;

namespace community.providers.community.Implementation;

/// <summary>
/// Exposes methods used for manipulating community data.
/// </summary>
/// <param name="contextAccessor">The HttpContext accessor is provided to the base class in order to access the underlying claims on the JWT.</param>
/// <param name="repository">Data access methods for Community records.</param>
public class CommunityProvider(
    IHttpContextAccessor contextAccessor,
    ICommunityRepository repository)
    : BaseProvider(contextAccessor), ICommunityProvider
{
    /// <inheritdoc />
    public async Task<MultiResponse<CommunityResponse>> SearchAsync(FindCommunityRequest request)
    {
        var response = await MeasureExecutionAsync(async () =>
        {
            var foundRecords = await repository.FindCommunityAsync(request);
            if (foundRecords == null) throw new NotFoundException(ErrorCodes.Search_NoResultsYielded);
            
            var communities= await repository.ListAsync(foundRecords.Select(r => r.Id).ToList());
            var responses = communities.Select(c => (CommunityResponse)c);
            return new MultiResponse<CommunityResponse>(responses);
        });

        return response;
    }

    /// <inheritdoc />
    public async Task<SingleResponse<CommunityResponse>> GetAsync(Guid id)
    {
        var response = await MeasureExecutionAsync(async () =>
        {
            var community = await repository.GetAsync(id);
            return new SingleResponse<CommunityResponse>(community);
        });
        
        return response;
    }
}