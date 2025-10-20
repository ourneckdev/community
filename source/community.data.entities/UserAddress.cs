using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace community.data.entities;

/// <summary>
///     Represents a many-to-many mapping of tenant/users to addresses.
/// </summary>
public class UserAddress : BaseAddressEntity
{
    /// <summary>
    ///     Gets or sets the id of the user being mapped.
    /// </summary>
    [Key]
    [Column("user_id")]
    public Guid UserId { get; set; }


    #region Navigation Properties

    /// <summary>
    ///     Navigation property.
    /// </summary>
    public User User { get; set; } = null!;

    #endregion
}