using System.Text.Json.Serialization;
using community.common.Definitions;
using community.common.Extensions;
using community.data.entities;
using community.models.Responses.Authentication;
using community.models.Responses.Base;

namespace community.models.Responses;

/// <summary>
///     Defines the available properties for the response object of a user.
/// </summary>
public class UserResponse : BasePrimaryResponse
{
    /// <summary>
    ///     Gets or sets the type of user, used to drive the assignment of claims.
    /// </summary>
    public Guid UserTypeId { get; set; }

    /// <summary>
    ///     Gets the text representation of the user's type
    /// </summary>
    public string UserType => UserTypes.Values[UserTypeId];

    /// <summary>
    ///     Gets or sets the user's username, the email address or phone number they signed up with
    /// </summary>
    public string Username { get; set; } = "";

    /// <summary>
    ///     Get or sets a flag whether the username has been verified.
    /// </summary>
    public bool UsernameVerified { get; set; }

    /// <summary>
    ///     Get or sets the date the username was verified.
    /// </summary>
    public DateTime? UsernameVerifiedDate { get; set; }

    /// <summary>
    ///     Gets or sets the user's name prefix (eg: Mr., Ms., Mrs.)
    /// </summary>
    public string? Prefix { get; set; }

    /// <summary>
    ///     Gets or sets the user's first name.
    /// </summary>
    public string FirstName { get; set; } = "";

    /// <summary>
    ///     Gets or sets the user's last name.
    /// </summary>
    public string LastName { get; set; } = "";

    /// <summary>
    ///     Gets or sets the user's name suffix (eg: Jr, II, III, etc).
    /// </summary>
    public string? Suffix { get; set; }

    /// <summary>
    ///     Gets or sets the user's date of birth.
    /// </summary>
    public DateOnly? DateOfBirth { get; set; }

    /// <summary>
    ///     Gets or sets an indicator whether the user's account is locked
    /// </summary>
    public bool Locked { get; set; }

    /// <summary>
    ///     Gets or sets the name of the object/file assigned to the user for their avatar.
    /// </summary>
    public string? ProfilePic { get; set; }

    /// <summary>
    ///     Gets or sets the last login date for the user.
    /// </summary>
    public DateTime? LastLoginDate { get; set; }

    /// <summary>
    ///     Gets the last selected community id.
    /// </summary>
    /// <remarks>This value is updated every time a community is swapped and can be considered the current community.</remarks>
    public Guid? LastCommunityId { get; set; }

    /// <summary>
    ///     Gets a list of the claims assigned to the user.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IEnumerable<ClaimResponse> Claims { get; set; } = new List<ClaimResponse>();

    /// <summary>
    ///     Gets an optional collection of contact methods defined to a user.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IEnumerable<ContactResponse>? ContactMethods { get; set; } = new List<ContactResponse>();

    /// <summary>
    ///     Gets or sets an optional list of addresses for a user.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IEnumerable<UserAddressResponse>? Addresses { get; set; } = new List<UserAddressResponse>();

    /// <summary>
    ///     Gets or sets the communities available to the user.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IEnumerable<CommunityResponse>? Communities { get; set; } = new List<CommunityResponse>();


    /// <summary>
    ///     Maps a user entity to a response object.
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public static implicit operator UserResponse(User user)
    {
        var userResponse = MapValues<UserResponse>(user);
        userResponse.UserTypeId = user.UserTypeId;
        userResponse.Username = user.Username;
        userResponse.UsernameVerified = user.UsernameVerified;
        userResponse.UsernameVerifiedDate = user.UsernameVerifiedDate;
        userResponse.Prefix = user.Prefix;
        userResponse.FirstName = user.FirstName;
        userResponse.LastName = user.LastName;
        userResponse.Suffix = user.Suffix;
        userResponse.DateOfBirth = user.DateOfBirth.FromEncryptedString();
        userResponse.Locked = user.Locked;
        userResponse.ProfilePic = user.ProfilePic;
        userResponse.LastLoginDate = user.LastLoginDate;
        userResponse.LastCommunityId = user.LastCommunityId;
        userResponse.ContactMethods = user.ContactMethods?.Any() ?? false
            ? user.ContactMethods.Select(m => (ContactResponse)m)
            : null;
        userResponse.Addresses = user.Addresses?.Any() ?? false
            ? user.Addresses.Select(a => (UserAddressResponse)a)
            : null;
        userResponse.Communities = user.Communities?.Any() ?? false
            ? user.Communities.Select(c => (CommunityResponse)c)
            : null;
        return userResponse;
    }
}