using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using community.common.BaseClasses;

namespace community.data.entities.Reports;

/// <summary>
///     Represents a comment on a report
/// </summary>
public class ReportComment : BaseCommunityEntity
{
    /// <summary>
    ///     The report the comment is related to.
    /// </summary>
    [Column("report_id")]
    public Guid ReportId { get; set; }

    /// <summary>
    ///     Resolves the parent comment in a threaded discussio
    /// </summary>
    [Column("parent_comment_id")]
    public Guid? ParentCommentId { get; set; }

    /// <summary>
    ///     Gets or sets an optioal subject of the comment.
    /// </summary>
    [Column("subject")]
    [MaxLength(200)]
    public string? Subject { get; set; }

    /// <summary>
    ///     Gets or sets the comment made.
    /// </summary>
    [Column("comment")]
    [MaxLength(2000)]
    public string Comment { get; set; } = "";


    /// <summary>
    ///     Navigation property.
    /// </summary>
    public Report Report { get; set; } = null!;

    /// <summary>
    ///     Navigation property.
    /// </summary>
    public ReportComment ParentComment { get; set; } = null!;
}