using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using community.common.BaseClasses;

namespace community.data.entities.Authorization;

/// <summary>
///     Defines a claim within the system.
/// </summary>
public class Claim : BasePrimaryEntity
{
    /// <summary>
    ///     The categorization the claim is assigned under, area of the application.
    /// </summary>
    [Column("claim_section_id")]
    public Guid ClaimSectionId { get; set; }

    /// <summary>
    ///     The name of the claim assigned as scope within the JWT, used for authorization.
    /// </summary>
    [Column("name")]
    [MaxLength(50)]
    public string Name { get; set; } = "";

    /// <summary>
    ///     Gets or sets the description of the claim.
    /// </summary>
    [Column("description")]
    [MaxLength(50)]
    public string Description { get; set; } = "";
}