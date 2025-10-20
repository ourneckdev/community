namespace community.models.BusinessObjects.DomainModels;

/// <summary>
/// Logic layer representation of a Community DDD Entity
/// </summary>
public class CommunityModel : BasePrimaryModel
{
    /// <summary>
    ///     Gets or sets the community name
    /// </summary>
    public string Name { get; set; } = "";

    /// <summary>
    ///     Gets or sets the optional description of the community.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    ///     Gets or sets an optional website.
    /// </summary>
    public string? Website { get; set; }

    /// <summary>
    ///     Gets or sets the parent, allowing for creating a hierarchical representation of communities
    /// </summary>
    public Guid? ParentId { get; set; }

    /// <summary>
    ///     Gets or sets the name of the S3 bucket where media is stored.
    /// </summary>
    public string S3BucketName { get; set; } = "";
    
    /// <summary>
    /// Optional community information for number of parcels
    /// </summary>
    public int? NumberOfParcels { get; set; }
    
    /// <summary>
    /// Optional community information for average parcel size
    /// </summary>
    public decimal? ParcelSize { get; set; }
    
    /// <summary>
    /// Optional community information for unit of parcel size
    /// </summary>
    public Guid? ParcelSizeUnitId { get; set; }
    
    /// <summary>
    /// Optional community information for number of residents 
    /// </summary>
    public int? NumberOfResidents { get; set; }
    
    /// <summary>
    /// Optional community information for average home value
    /// </summary>
    public decimal? AverageHomeValue { get; set; }
}