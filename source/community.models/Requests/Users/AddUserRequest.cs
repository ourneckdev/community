using community.common.Utilities;
using community.data.entities;
using community.models.Abstract;
using community.models.Requests.Addresses;
using community.models.Requests.ContactMethods;

namespace community.models.Requests.Users;

/// <summary>
/// 
/// </summary>
/// <param name="CommunityId"></param>
/// <param name="UserTypeId"></param>
/// <param name="Password"></param>
/// <param name="Prefix"></param>
/// <param name="FirstName"></param>
/// <param name="LastName"></param>
/// <param name="Suffix"></param>
/// <param name="DateOfBirth"></param>
/// <param name="Addresses"></param>
/// <param name="ContactMethods"></param>
public record AddUserRequest(
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
    /// Converts the immutable record to a user object for save purposes
    /// </summary>
    /// <returns></returns>
    public User ToUser()
    {   
        return new User
        {
            UserTypeId = UserTypeId,
            Password = Password,
            Prefix = Prefix,
            FirstName = FirstName,
            LastName = LastName,
            Suffix = Suffix,
            DateOfBirth = DateOfBirth.ToEncryptedString(),
            ContactMethods = ContactMethods?.Select(c => c.ToContact(CommunityId)).ToList() ?? [],
            Addresses = Addresses?.Select(a => a.ToUserAddress()).ToList() ?? []
        };
    }
}
