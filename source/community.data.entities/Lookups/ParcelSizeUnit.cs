using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using community.common.BaseClasses;

namespace community.data.entities.Lookups;

/// <summary>
///     Lookup value for parcel units to display in dropdown lists.
/// </summary>
[Table("parcel_size_unit")]
public class ParcelSizeUnit : BaseLookupEntity
{
    /// <summary>
    ///     Gets or sets the description of the lookup value
    /// </summary>
    [MaxLength(200)]
    public string? Description { get; set; }
}