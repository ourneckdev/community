namespace community.models.BusinessObjects.DomainModels;

/// <summary>
/// Defines a primary community domain object. 
/// </summary>
public abstract class BaseCommunityModel : BasePrimaryModel
{
    /// <summary>
    /// Gets or sets the community id of the object.
    /// </summary>
    public Guid CommunityId { get; set; }
}