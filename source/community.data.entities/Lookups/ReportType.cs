using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using community.common.BaseClasses;

namespace community.data.entities.Lookups;

/// <summary>
///     Defines the types of available reports to initiate.
/// </summary>
[Table("report_type")]
public class ReportType : BaseLookupEntity
{
    /// <summary>
    ///     Gets or sets the name of the image to display as the icon for the report.
    /// </summary>
    [MaxLength(255)]
    public string Icon { get; set; } = "";
    
    /// <summary>
    /// Gets or sets the order the type should display in the UI
    /// </summary>
    public short SortOrder { get; set; }
}