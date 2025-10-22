using System.ComponentModel.DataAnnotations.Schema;
using community.common.BaseClasses;

namespace community.data.entities.Lookups;

/// <summary>
///     Assigns
/// </summary>
[Table("contact_method")]
public sealed class ContactMethod : BaseLookupCommunityEntity
{
    /// <summary>
    ///     Gets or sets an enumeration indicating whether the contact method is a phone or email.
    /// </summary>
    public string ContactType { get; set; } = null!;
}