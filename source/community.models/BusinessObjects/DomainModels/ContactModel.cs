using community.common.Enumerations;
using community.data.entities;

namespace community.models.BusinessObjects.DomainModels;

/// <summary>
/// 
/// </summary>
public class ContactModel : BaseCommunityModel
{
    /// <summary>
    ///     Gets or sets the id of the user the contact record is related
    /// </summary>
    public Guid? UserId { get; set; }
    
    /// <summary>
    /// 
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


    /// <summary>
    /// Maps a database object to a domain business object.
    /// </summary>
    /// <param name="contact"></param>
    /// <returns></returns>
    public static implicit operator ContactModel(Contact contact)
    {
        return new()
        {
            Id = contact.Id,
            CommunityId = contact.CommunityId,
            UserId = contact.UserId,
            EntityType = contact.EntityType,
            ContactMethodId = contact.ContactMethodId,
            Value = contact.Value,
            Verified = contact.Verified,
            VerifiedDate = contact.VerifiedDate,
            Visible = contact.Visible,
            CanContact = contact.CanContact,
            CreatedDate = contact.CreatedDate,
            ModifiedDate = contact.ModifiedDate,
            CreatedBy = contact.CreatedBy,
            ModifiedBy = contact.ModifiedBy,
            IsActive = contact.IsActive
        };
    }
    /// <summary>
    /// Maps a database object to a domain business object.
    /// </summary>
    /// <param name="contact"></param>
    /// <returns></returns>
    public static implicit operator Contact(ContactModel contact)
    {
        return new()
        {
            Id = contact.Id,
            CommunityId = contact.CommunityId,
            UserId = contact.UserId,
            ContactMethodId = contact.ContactMethodId,
            Value = contact.Value,
            Verified = contact.Verified,
            VerifiedDate = contact.VerifiedDate,
            Visible = contact.Visible,
            CanContact = contact.CanContact,
            CreatedDate = contact.CreatedDate,
            ModifiedDate = contact.ModifiedDate,
            CreatedBy = contact.CreatedBy,
            ModifiedBy = contact.ModifiedBy,
            IsActive = contact.IsActive
        };
    }
}