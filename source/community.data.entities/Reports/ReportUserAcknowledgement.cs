using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using community.common.BaseClasses;

namespace community.data.entities.Reports;

/// <summary>
///     Represents a log of acknowledgements by a community member.
/// </summary>
public class ReportUserAcknowledgement : BaseEntity
{
    /// <summary>
    ///     Gets or sets the report that's being acknowledged.
    /// </summary>
    [Key]
    [Column("report_id")]
    public Guid ReportId { get; set; }

    /// <summary>
    ///     Gets or sets the user associated with the acknowledgement
    /// </summary>
    [Key]
    [Column("user_id")]
    public Guid UserId { get; set; }

    /// <summary>
    ///     Gets or sets the community the user and report.
    /// </summary>
    [Key]
    [Column("community_id")]
    public Guid CommunityId { get; set; }

    /// <summary>
    ///     Gets or sets the date the report was acknowledged.
    /// </summary>
    [Column("acknowledged_date")]
    public DateTime AcknowledgedDate { get; set; }


    /// <summary>
    ///     Navigation property.
    /// </summary>
    public User User { get; set; } = null!;

    /// <summary>
    ///     Navigation property.
    /// </summary>
    public Community Community { get; set; } = null!;

    /// <summary>
    ///     Navigation property.
    /// </summary>
    public Report Report { get; set; } = null!;
}