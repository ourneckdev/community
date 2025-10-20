using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using community.common.BaseClasses;

namespace community.data.entities.Locales;

/// <summary>
///     Represents a single row
/// </summary>
public class Country : BaseLocaleEntity
{
    /// <summary>
    ///     Gets or sets the two digit Iso Code
    /// </summary>
    [Length(2, 2)]
    [Column("iso_2")]
    public string Iso2Code { get; set; } = "";

    /// <summary>
    ///     Gets or sets the 3 digit numeric Code
    /// </summary>
    [Length(3, 3)]
    [Column("numeric_code  ")]
    public string NumericCode { get; set; } = "";

    /// <summary>
    ///     Gets or sets the sort order to use to retrieve the results
    /// </summary>
    public short? SortOrder { get; set; }
}