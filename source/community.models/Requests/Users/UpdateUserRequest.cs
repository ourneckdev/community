using community.common.Utilities;
using community.data.entities;
using community.models.Abstract;
using community.models.Requests.Addresses;
using community.models.Requests.ContactMethods;

namespace community.models.Requests.Users;

/// <summary>
///     Encapsulates all the required and optional properties required during the registration process.
/// </summary>
/// <param name="Id">Gets or sets the user id.</param>
/// <param name="CommunityId">The id of the address record being edited</param>
/// <param name="UserTypeId">
///     Gets the user type, should be set to CommunityModel Admin during the CommunityModel
///     Registration process, or CommunityModel Member for a user sign up.
/// </param>
/// <param name="Password">Optional, the password the user has chosen.</param>
/// <param name="Prefix">The user's prefix, (eg: Mr. Mrs.)</param>
/// <param name="FirstName">Users firstname</param>
/// <param name="LastName">UserModel's lastname</param>
/// <param name="Suffix">Optional, suffix (eg: Jr, Sr)</param>
/// <param name="DateOfBirth">The user's date of birth</param>
/// <param name="Addresses"></param>
/// <param name="ContactMethods"></param>
public record UpdateUserRequest(
    Guid Id,
    Guid CommunityId,
    Guid UserTypeId,
    string? Password,
    string? Prefix,
    string FirstName,
    string LastName,
    string? Suffix,
    DateOnly? DateOfBirth,
    IEnumerable<EditAddressRequest>? Addresses,
    IEnumerable<ContactMethodRequest>? ContactMethods) : VerifiableUserNameRecord
{
    /// <summary>
    ///     Converts the immutable record to a user object for save purposes
    /// </summary>
    /// <returns></returns>
    public User ToUser()
    {
        return new User
        {
            Id = Id,
            UserTypeId = UserTypeId,
            Password = Password,
            Prefix = Prefix,
            FirstName = FirstName,
            LastName = LastName,
            Suffix = Suffix,
            DateOfBirth = DateOfBirth.ToEncryptedString(),
            ContactMethods = ContactMethods?.Select(c => c.ToContact(CommunityId, Id)).ToList() ?? [],
            Addresses = Addresses?.Select(a => a.ToUserAddress()).ToList() ?? []
        };
    }
}