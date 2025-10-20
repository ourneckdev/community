namespace community.models.Abstract;

/// <summary>
/// defines request objects that are required to supply a community id 
/// </summary>
public abstract record BaseCommunityRecord
{
    /// <summary>
    /// 
    /// </summary>
    public Guid CommunityId { get; init; }
};