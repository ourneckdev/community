using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace community.common.BaseClasses;

/// <summary>
///     Primary entities are used to define records that have a standard ID field.
/// </summary>
public abstract class BasePrimaryEntity : BaseEntity
{
    /// <summary>
    ///     Gets or sets the unique identifier of the record.
    /// </summary>
    [Key]
    [Column("id")]
    public Guid Id { get; set; }
}