using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using community.common.BaseClasses;

[assembly: InternalsVisibleTo("community.data.postgres")]
[assembly: InternalsVisibleTo("community.tests.common")]

namespace community.data.entities;

/// <summary>
///     The Community is the tenant level assignment for all users, the base level of information.
/// </summary>
public sealed class Community : BasePrimaryEntity
{
    /// <summary>
    /// Default constructor for a community
    /// </summary>
    public Community() {  }

    internal Community(Community community)
    {
        Id = community.Id;
        Name = community.Name;
        Description = community.Description;
        Website = community.Website;
        ParentId = community.ParentId;
        S3BucketName = community.S3BucketName;
        NumberOfParcels = community.NumberOfParcels;
        ParcelSize = community.ParcelSize;
        ParcelSizeUnitId = community.ParcelSizeUnitId;
        NumberOfResidents = community.NumberOfResidents;
        AverageHomeValue = community.AverageHomeValue;
        Addresses = community.Addresses;
        ContactMethods = community.ContactMethods;
    }

    internal Community(Community community, IEnumerable<CommunityAddress> addresses)
        : this(community)
    {
        Addresses = addresses;
    }

    internal Community(Community community, IEnumerable<Contact> contactMethods)
        : this(community)
    {
        ContactMethods = contactMethods;
    }

    /// <summary>
    ///     Gets or sets the community name
    /// </summary>
    [Column("name")]
    [MaxLength(200)]
    public string Name { get; set; } = "";

    /// <summary>
    ///     Gets or sets the optional description of the community.
    /// </summary>
    [Column("description")]
    public string? Description { get; set; }

    /// <summary>
    ///     Gets or sets an optional website.
    /// </summary>
    [Column("website")]
    [MaxLength(255)]
    public string? Website { get; set; }

    /// <summary>
    ///     Gets or sets the parent, allowing for creating a hierarchical representation of communities
    /// </summary>
    [Column("parent_id")]
    public Guid? ParentId { get; set; }

    /// <summary>
    ///     Gets or sets the name of the S3 bucket where media is stored.
    /// </summary>
    [Column("s3_bucket_name")]
    [MaxLength(255)]
    public string S3BucketName { get; set; } = "";

    /// <summary>
    ///     Optional community information for number of parcels
    /// </summary>
    public int? NumberOfParcels { get; set; }

    /// <summary>
    ///     Optional community information for average parcel size
    /// </summary>
    public decimal? ParcelSize { get; set; }

    /// <summary>
    ///     Optional community information for unit of parcel size
    /// </summary>
    public Guid? ParcelSizeUnitId { get; set; }

    /// <summary>
    ///     Optional community information for number of residents
    /// </summary>
    public int? NumberOfResidents { get; set; }

    /// <summary>
    ///     Optional community information for average home value
    /// </summary>
    public decimal? AverageHomeValue { get; set; }


    /// <summary>
    ///     Optionally defined addresses specific to the community.
    /// </summary>
    public IEnumerable<CommunityAddress>? Addresses { get; set; } = new List<CommunityAddress>();

    /// <summary>
    ///     Optionally defined contact methods specific to the community.
    /// </summary>
    public IEnumerable<Contact>? ContactMethods { get; set; } = new List<Contact>();
}