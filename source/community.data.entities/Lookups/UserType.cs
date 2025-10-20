using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using community.common.BaseClasses;

namespace community.data.entities.Lookups;

/// <summary>
///     User types are system level information, used for assessing which claims are relevant to the user.
/// </summary>
[Table("user_type")]
public class UserType : BaseLookupEntity
{
    /// <summary>
    ///     Gets or sets a description of the purpose for each type.
    /// </summary>
    [Column("description")]
    [MaxLength(200)]
    public string Description { get; set; } = "";
}