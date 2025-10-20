using System.ComponentModel.DataAnnotations;

namespace community.common.BaseClasses;

/// <summary>
///     Defines base properties for lookup tables
/// </summary>
public abstract class BaseLookupEntity : BasePrimaryEntity
{
    /// <summary>
    ///     Gets or sets the name of the Address type
    /// </summary>
    [MaxLength(50)]
    public string Name { get; set; } = "";
}