using System.ComponentModel.DataAnnotations;
using community.common.BaseClasses;
using community.common.Enumerations;
using community.data.entities.Lookups;

namespace community.data.entities;

/// <summary>
///     Can store contact information for a user, either email or phone
/// </summary>
public class Contact : BaseCommunityEntity
{
    /// <summary>
    ///     Gets or sets the id of the user the contact record is related
    /// </summary>
    public Guid? UserId { get; set; }
    
    /// <summary>
    /// Relate the record to an individual user or a community.
    /// </summary>
    public EntityType EntityType { get; set; }

    /// <summary>
    ///     Gets or sets the type of contact record, phone/email
    /// </summary>
    /// <remarks>
    ///     Available types:
    ///     Mobile Phone, Home Phone, Work Phone
    ///     Emergency Contact, Personal Email, Work Email
    /// </remarks>
    public Guid ContactMethodId { get; set; }

    /// <summary>
    ///     Gets or sets the contact info, phone number or email address
    /// </summary>
    [MaxLength(200)]
    public string Value { get; set; } = "";

    /// <summary>
    ///     Gets or sets a flag indicating if the contact info is verified
    /// </summary>
    public bool Verified { get; set; }

    /// <summary>
    ///     Gets or sets the date the user contact method was verified.
    /// </summary>
    public DateTime? VerifiedDate { get; set; }
    
    /// <summary>
    /// Gets a flag indicating whether the contact method is displayed on a user's profile.
    /// </summary>
    public bool Visible { get; set; }

    /// <summary>
    ///     Gets or sets a flag indicating if the user can be contacted at the relevant phone or email.
    /// </summary>
    public bool CanContact { get; set; }
}
