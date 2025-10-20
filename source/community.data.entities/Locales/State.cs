using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using community.common.BaseClasses;

namespace community.data.entities.Locales;

/// <summary>
///     Represents a state in the state lookup table.
/// </summary>
public class State : BaseLocaleEntity
{
    /// <summary>
    ///     Gets or sets the code of the country the state resides in.
    /// </summary>
    [Length(3, 3)]
    [Column("country_code")]
    public string CountryCode { get; set; } = "";

    /// <summary>
    ///     Gets or sets the 2 digit FIPS code for the state
    /// </summary>
    [Length(2, 2)]
    [Column("fips_code")]
    public string FipsCode { get; set; } = "";

    /// <summary>
    ///     Gets or sets the state's ANSI code.
    /// </summary>
    [Length(2, 2)]
    [Column("ansi_code")]
    public string AnsiCode { get; set; } = "";
}