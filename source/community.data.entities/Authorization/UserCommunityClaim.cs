using System.ComponentModel.DataAnnotations;
using community.common.BaseClasses;

namespace community.data.entities.Authorization;

/// <summary>
///     Defines the mapping between the UserCommunity relationsh
///     ip and assigned claims for a user.
/// </summary>
public class UserCommunityClaim : BaseEntity
{
    /// <summary>
    ///     Gets or sets the user id assignee of the claim.
    /// </summary>
    [Key]
    public Guid UserId { get; set; }

    /// <summary>
    ///     Gets the community the user's claim is relevant for.
    /// </summary>
    [Key]
    public Guid CommunityId { get; set; }

    /// <summary>
    ///     Gets or sets the claim
    /// </summary>
    [Key]
    public Guid ClaimId { get; set; }
}