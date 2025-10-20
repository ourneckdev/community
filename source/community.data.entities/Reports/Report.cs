using community.common.BaseClasses;
using community.data.entities.Lookups;

namespace community.data.entities.Reports;

/// <summary>
///     Logs a report made by a community member
/// </summary>
public class Report : BaseCommunityEntity
{
    /// <summary>
    ///     The type of the report.
    /// </summary>
    public Guid ReportTypeId { get; set; }

    /// <summary>
    ///     The priority of the report.
    /// </summary>
    public short Priority { get; set; }

    /// <summary>
    ///     Defines if a report can be closed or stays displayed.
    /// </summary>
    public bool Sticky { get; set; }

    /// <summary>
    ///     Defines start date of scheduled reports
    /// </summary>
    public DateTime? ScheduleDate { get; set; }

    /// <summary>
    ///     For scheduled reports, defines an end date
    /// </summary>
    public DateTime? EndDate { get; set; }

    /// <summary>
    ///     the longitude coordinate.
    /// </summary>
    public decimal? Longitude { get; set; }

    /// <summary>
    ///     the latitude coordinate.
    /// </summary>
    public decimal? Latitude { get; set; }

    /// <summary>
    ///     the longitude coordinate.
    /// </summary>
    public decimal? EndLongitude { get; set; }

    /// <summary>
    ///     the latitude coordinate.
    /// </summary>
    public decimal? EndLatitude { get; set; }

    /// <summary>
    ///     An optional message to include in the report.
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    ///     A flag indicating if the report has been resolved.
    /// </summary>
    public bool Resolved { get; set; }

    /// <summary>
    ///     The date the report was resolved.
    /// </summary>
    public DateTime? ResolvedDate { get; set; }

    /// <summary>
    ///     The user who resolved the report.
    /// </summary>
    public Guid? ResolvedBy { get; set; }

    /// <summary>
    ///     Navigation property for the report type
    /// </summary>
    public ReportType ReportEntity { get; set; } = null!;

    /// <summary>
    ///     Navigation property for who resovled the report.
    /// </summary>
    public User? ResolvedByUser { get; set; }
}