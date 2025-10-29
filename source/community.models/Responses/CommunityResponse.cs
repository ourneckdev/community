using System.Text.Json.Serialization;
using community.data.entities;
using community.data.entities.Lookups;
using community.models.Responses.Base;
using community.models.Responses.Lookups;

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
    public Guid? ParentId { get; set; }

    /// <summary>
    ///     Gets or sets the bucket name in S3 any media assigned to the community
    /// </summary>
    public string? S3BucketName { get; set; }
    
    /// <summary>
    /// Returns any associated addresses for the community.
    /// </summary>
    public IEnumerable<CommunityAddressResponse>? Addresses { get; set; }
    
    /// <summary>
    /// Returns any associated email and phone numbers for the community.
    /// </summary>
    public IEnumerable<ContactResponse>? Contacts { get; set; }

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
        if (community.Addresses != null) 
            response.Addresses = community.Addresses.Select(a => (CommunityAddressResponse)a);
        if (community.ContactMethods != null)
            response.Contacts = community.ContactMethods.Select(c => (ContactResponse)c);
        return response;
    }
}