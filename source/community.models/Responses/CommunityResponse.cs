using System.Text.Json.Serialization;
using community.data.entities;
using community.models.Responses.Base;

namespace community.models.Responses;

/// <summary>
///     Defines the available parameters to map a community entity record to.
/// </summary>
public class CommunityResponse : BasePrimaryResponse
{
    /// <summary>
    ///     Gets or sets the community name
    /// </summary>
    public string Name { get; set; } = "";

    /// <summary>
    ///     Gets or set a description of the community.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    ///     Gets or sets the website for the community.
    /// </summary>
    public string? Website { get; set; }

    /// <summary>
    ///     Gets or sets an optional parent, if part of a hierarchy of communities.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Guid? ParentId { get; set; }

    /// <summary>
    ///     Gets or sets the bucket name in S3 any media assigned to the community
    /// </summary>
    public string? S3BucketName { get; set; }

    /// <summary>
    ///     Maps an entity to a response.
    /// </summary>
    /// <param name="community">the database entity</param>
    /// <returns>A hydrated object with the available parameters.</returns>
    public static implicit operator CommunityResponse(Community community)
    {
        var response = MapValues<CommunityResponse>(community);
        response.Name = community.Name;
        response.Description = community.Description;
        response.Website = community.Website;
        response.ParentId = community.ParentId;
        response.S3BucketName = community.S3BucketName;
        return response;
    }
}