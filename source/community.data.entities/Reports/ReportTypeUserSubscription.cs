using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using community.common.BaseClasses;
using community.common.Enumerations;
using community.data.entities.Lookups;

namespace community.data.entities.Reports;

/// <summary>
///     Allows a user to subscribe to, or unsubscribe from specific report types.
/// </summary>
public class ReportTypeUserCommunitySubscription : BaseEntity
{
    /// <summary>
    ///     Defines the report type a user wishes to subscribe.
    /// </summary>
    [Key]
    [Column("report_type_id")]
    public Guid ReportTypeId { get; set; }

    /// <summary>
    ///     Identifies the user who is subscribing or opting out of the report type
    /// </summary>
    [Key]
    [Column("user_id")]
    public Guid UserId { get; set; }

    /// <summary>
    ///     Gets or sets the community the report subscription is related to.
    /// </summary>
    [Key]
    [Column("community_id")]
    public Guid CommunityId { get; set; }

    /// <summary>
    ///     Defines the <see cref="NotificationType" />s the user has subscribed to.
    /// </summary>
    [Column("notification_type")]
    public NotificationType NotificationType { get; set; }

    /// <summary>
    ///     Navigation property for the user.
    /// </summary>
    public User User { get; set; } = null!;

    /// <summary>
    ///     Navigation property for the community
    /// </summary>
    public Community Community { get; set; } = null!;

    /// <summary>
    ///     Navigation property for the Report Type.s
    /// </summary>
    public ReportType ReportEntity { get; set; } = null!;
}