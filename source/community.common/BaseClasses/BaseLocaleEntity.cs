using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace community.common.BaseClasses;

/// <summary>
///     Abstract class used to define standard columns on Lookup tables.
/// </summary>
public abstract class BaseLocaleEntity : BaseEntity
{
    /// <summary>
    ///     Gets or sets the code used as primary key.
    /// </summary>
    [Key]
    [Column("code")]
    public string Code { get; set; } = "";

    /// <summary>
    ///     Gets or sets the name of the lookupable item.
    /// </summary>
    [MaxLength(100)]
    [Column("name")]
    public string Name { get; set; } = "";
}