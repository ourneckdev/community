using System.Text.Json.Serialization;
using community.common.Definitions;
using community.common.Enumerations;
using community.data.entities;
using community.models.Responses.Base;

namespace community.models.Responses;

/// <summary>
///     Defines individual user contact methods response
/// </summary>
public class UserContactMethodResponse : BasePrimaryResponse
{
    /// <summary>
    ///     Gets or sets the type of contact record, phone/email
    /// </summary>
    /// <remarks>
    ///     Available types:
    ///     Mobile Phone, Home Phone, Work Phone
    ///     Emergency Contact, Personal Email, Work Email
    /// </remarks>
    public string ContactMethod { get; set; } = "";

    /// <summary>
    ///     Gets or sets the contact info, phone number or email address
    /// </summary>
    public string Value { get; set; } = "";

    /// <summary>
    ///     Gets or sets a flag indicating if the contact info is verified
    /// </summary>
    public bool Verified { get; set; }

    /// <summary>
    ///     Gets or sets an optional date when the method was verified.
    /// </summary>
    public DateTime? VerifiedDate { get; set; }

    /// <summary>
    ///     Gets or sets a flag indicating if the user can be contacted at the relevant phone or email.
    /// </summary>
    public bool CanContact { get; set; }

    /// <summary>
    ///     Gets or sets the contact type
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ContactType ContactType { get; set; }

    /// <summary>
    ///     Maps a database level user contact method entity to a response object.
    /// </summary>
    /// <param name="contact">the database entity to be mapped.</param>
    /// <returns>a hydrated response object.</returns>
    public static implicit operator UserContactMethodResponse(Contact contact)
    {
        return new UserContactMethodResponse
        {
            Id = contact.Id,
            ContactMethod = ContactMethods.Values[contact.ContactMethodId].Item1,
            ContactType = ContactMethods.Values[contact.ContactMethodId].Item2,
            Value = contact.Value,
            Verified = contact.Verified,
            VerifiedDate = contact.VerifiedDate,
            CanContact = contact.CanContact,
            CreatedBy = contact.CreatedBy,
            ModifiedBy = contact.ModifiedBy,
            CreatedDate = contact.CreatedDate,
            ModifiedDate = contact.ModifiedDate,
            IsActive = contact.IsActive
        };
    }
}