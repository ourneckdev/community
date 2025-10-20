using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using community.common.BaseClasses;

namespace community.data.entities.Authorization;

/// <summary>
///     categorizes the different available claims.  some sections will be completely invisible to admins based on user
///     type.
/// </summary>
public class ClaimSection : BasePrimaryEntity
{
    /// <summary>
    ///     The name of the section a claim assigned under, used for authorization.
    /// </summary>
    [Column("name")]
    [MaxLength(50)]
    public string Name { get; set; } = "";

    /// <summary>
    ///     Gets or sets the description of the section.
    /// </summary>
    [Column("description")]
    [MaxLength(50)]
    public string Description { get; set; } = "";


    /// <summary>
    ///     Gets the list of claims assgined to the section.
    /// </summary>
    public IEnumerable<Claim> Claims { get; set; } = new List<Claim>();
}