using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using community.common.BaseClasses;
using community.data.entities.Lookups;

namespace community.data.entities.Authorization;

/// <summary>
///     Assigns one or more claim sections to a user type.
/// </summary>
public class UserTypeClaimSection : BaseEntity
{
    /// <summary>
    ///     Gets or sets the user type assigned to the collection of claims.
    /// </summary>
    [Key]
    [Column("user_type_id")]
    public Guid UserTypeId { get; set; }

    /// <summary>
    ///     Gets or sets the section the user type is being related to.
    /// </summary>
    [Key]
    [Column("claim_section_id")]
    public Guid ClaimSectionId { get; set; }

    /// <summary>
    ///     Navigation property for user types
    /// </summary>
    public UserType UserType { get; set; } = null!;

    /// <summary>
    ///     Navigation properties for claim sections.
    /// </summary>
    public ClaimSection ClaimSection { get; set; } = null!;
}