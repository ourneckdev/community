using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace community.common.BaseClasses;

/// <summary>
///     Community entities are tied to a Community multi-tenancy key
/// </summary>
public abstract class BaseCommunityEntity : BasePrimaryEntity
{
    /// <summary>
    ///     Gets or sets the unique identifier of the community tenancy record.
    /// </summary>
    [Key]
    [Column("community_id")]
    public Guid CommunityId { get; set; }
}