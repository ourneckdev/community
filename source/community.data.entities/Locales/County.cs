using System.ComponentModel.DataAnnotations;
using community.common.BaseClasses;

namespace community.data.entities.Locales;

/// <summary>
///     Represents a single row from the county table
/// </summary>
public class County : BaseLocaleEntity
{
    /// <summary>
    ///     Gets or sets the State Code
    /// </summary>
    [Length(2, 2)]
    public string StateCode { get; set; } = null!;

    /// <summary>
    /// </summary>
    [Length(3, 3)]
    public string CountryCode { get; set; } = null!;
}