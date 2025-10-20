using System.Text.Json.Serialization;
using community.common.BaseClasses;

namespace community.models.Responses.Base;

/// <summary>
///     Represents and response that should be targeted to a specific community
/// </summary>
public abstract class BaseCommunityResponse : BasePrimaryResponse
{
    /// <summary>
    ///     Gets or sets the related community for a response object
    /// </summary>
    public Guid CommunityId { get; set; }

    /// <summary>
    ///     Gets or sets the base community data to return in the resultset.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public CommunityResponse Community { get; set; } = null!;


    /// <summary>
    ///     Maps a community entity to a response object.
    /// </summary>
    /// <param name="baseEntity">The base entity to be mapped.</param>
    /// <typeparam name="T">The implementing type</typeparam>
    /// <returns>A hydrated community object.</returns>
    protected static T MapValues<T>(BaseCommunityEntity baseEntity) where T : BaseCommunityResponse, new()
    {
        var communityResponse = BasePrimaryResponse.MapValues<T>(baseEntity);
        communityResponse.CommunityId = baseEntity.CommunityId;
        return communityResponse;
    }
}