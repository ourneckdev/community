using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using community.common.BaseClasses;

namespace community.data.entities;

/// <summary>
///     Lookup table that links a user to one or more communities
/// </summary>
public class UserCommunity : BaseEntity
{
    /// <summary>
    ///     Gets or sets the Id of the user in question
    /// </summary>
    [Key]
    public Guid UserId { get; set; }

    /// <summary>
    ///     Gets or sets the community the user is a membmer of
    /// </summary>
    [Key]
    public Guid CommunityId { get; set; }

    /// <summary>
    ///     Gets or sets an indicator as to whether the user is a verified member of the community.
    /// </summary>
    [Column("verified")]
    public bool Verified { get; set; }

    /// <summary>
    ///     Gets or sets the user who did the verification, if verified.
    /// </summary>
    [Column("verified_by")]
    public Guid? VerifiedBy { get; set; }

    /// <summary>
    ///     Gets or sets the date the user was verified, if verified.
    /// </summary>
    [Column("verified_date")]
    public DateTime? VerifiedDate { get; set; }


    #region Navigation Properties

    /// <summary>
    ///     Navigation property.
    /// </summary>
    public User User { get; set; } = null!;

    /// <summary>
    ///     Navigation property.
    /// </summary>
    public Community Community { get; set; } = null!;

    #endregion
}