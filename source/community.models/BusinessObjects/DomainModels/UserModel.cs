using community.common.Definitions;
using community.common.Extensions;
using community.data.entities;
using community.models.Requests.Registration;
using community.models.Requests.Users;

namespace community.models.BusinessObjects.DomainModels;

/// <summary>
///     Represents a user object
/// </summary>
public class UserModel : BasePrimaryModel
{
    /// <summary>
    ///     Gets or sets the type of the user
    /// </summary>
    /// <remarks>
    ///     SiteAdmin
    ///     SupportAdmin
    ///     CommunityAdmin
    ///     CommunityMember
    /// </remarks>
    public Guid UserTypeId { get; set; }

    /// <summary>
    ///     Gets or sets the username, either email or mobile phone, of the user.
    /// </summary>
    public string Username { get; set; } = "";

    /// <summary>
    ///     Gets or sets an optional password that can be used to login, if preferred.
    /// </summary>
    public string? Password { get; init; } = "";

    /// <summary>
    ///     Gets or sets an indicator whether the user is a verified member of the community.
    /// </summary>
    public bool UsernameVerified { get; init; }

    /// <summary>
    ///     Gets or sets the date the user was verified, if verified.
    /// </summary>
    public DateTime? UsernameVerifiedDate { get; init; }

    /// <summary>
    ///     Gets or sets the temporary password code used for logging in.
    /// </summary>
    /// <remarks>
    ///     While this column exists it will never be selected or set by object reference.  It will be used in query
    ///     predicates only during login.
    /// </remarks>
    public string? LoginCode { get; init; }

    /// <summary>
    ///     Get or sets the expiration time for the generated password code.
    /// </summary>
    /// <remarks>
    ///     While this column exists it will never be selected or set by object reference.  It will be used in query
    ///     predicates only during login.
    /// </remarks>
    public DateTime? LoginCodeExpiration { get; init; }

    /// <summary>
    ///     Gets or sets the user's name prefix.
    /// </summary>
    public string? Prefix { get; set; }

    /// <summary>
    ///     Gets or sets the user's first name
    /// </summary>
    public string FirstName { get; set; } = "";

    /// <summary>
    ///     Gets or sets the user's lastname.
    /// </summary>
    public string LastName { get; set; } = "";

    /// <summary>
    ///     Gets or sets the user's name prefix.
    /// </summary>
    public string? Suffix { get; set; }

    /// <summary>
    ///     Gets or sets the user's date of birth.
    /// </summary>
    public DateOnly? DateOfBirth { get; set; }

    /// <summary>
    ///     Gets or sets a status indicating whether the user's account is locked out.
    /// </summary>
    /// <remarks>Locked accounts are a result of too many failed login attempts.</remarks>
    public bool Locked { get; set; }

    /// <summary>
    ///     Gets or sets the object name in S3
    /// </summary>
    public string? ProfilePic { get; set; }

    /// <summary>
    ///     Gets or sets the timestamp the last time the user logged in
    /// </summary>
    public DateTime? LastLoginDate { get; set; }

    /// <summary>
    ///     Gets or sets the last community the user logged into
    /// </summary>
    public Guid? LastCommunityId { get; set; }

    /// <summary>
    ///     Gets the email addresses and phone numbers for the user.
    /// </summary>
    public IEnumerable<ContactModel>? ContactMethods { get; set; } = new List<ContactModel>();

    /// <summary>
    ///     Gets the addresses for the user.
    /// </summary>
    public IEnumerable<UserAddressModel>? Addresses { get; set; } = new List<UserAddressModel>();


    /// <summary>
    ///     Maps a database user object to a domain entity object.
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public static implicit operator UserModel(User user)
    {
        return new UserModel
        {
            Id = user.Id,
            UserTypeId = user.UserTypeId,
            Username = user.Username,
            Password = user.Password,
            UsernameVerified = user.UsernameVerified,
            UsernameVerifiedDate = user.UsernameVerifiedDate,
            LoginCode = user.LoginCode,
            LoginCodeExpiration = user.LoginCodeExpiration,
            Prefix = user.Prefix,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Suffix = user.Suffix,
            DateOfBirth = user.DateOfBirth.FromEncryptedString(),
            LastLoginDate = user.LastLoginDate,
            LastCommunityId = user.LastCommunityId,
            CreatedDate = user.CreatedDate,
            ModifiedDate = user.ModifiedDate,
            CreatedBy = user.CreatedBy,
            ModifiedBy = user.ModifiedBy,
            IsActive = user.IsActive,
            Addresses = user.Addresses?.Select(a => (UserAddressModel)a),
            ContactMethods = user.ContactMethods?.Select(a => (ContactModel)a)
        };
    }

    /// <summary>
    ///     Maps the entity back to a database entity
    /// </summary>
    /// <returns></returns>
    public static implicit operator User(UserModel user)
    {
        return new User
        {
            Id = user.Id,
            UserTypeId = user.UserTypeId,
            Username = user.Username,
            Password = user.Password,
            UsernameVerified = user.UsernameVerified,
            UsernameVerifiedDate = user.UsernameVerifiedDate,
            LoginCode = user.LoginCode,
            LoginCodeExpiration = user.LoginCodeExpiration,
            Prefix = user.Prefix,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Suffix = user.Suffix,
            DateOfBirth = user.DateOfBirth.ToEncryptedString(),
            LastLoginDate = user.LastLoginDate,
            LastCommunityId = user.LastCommunityId,
            CreatedDate = user.CreatedDate,
            ModifiedDate = user.ModifiedDate,
            CreatedBy = user.CreatedBy,
            ModifiedBy = user.ModifiedBy,
            IsActive = user.IsActive,
            Addresses = user.Addresses?.Select(a => (UserAddress)a),
            ContactMethods = user.ContactMethods?.Select(c => (Contact)c)
        };
    }

    /// <summary>
    /// </summary>
    /// <param name="request">The request object to apply changes over the top of the database entity.</param>
    /// <param name="userId">The ID of the user changing the record.</param>
    public async Task ApplyChanges(UpdateUserRequest request, Guid? userId)
    {
        var hash = await GetHash();

        if (!Id.Equals(request.Id))
            Id = request.Id;

        if (!UserTypeId.Equals(request.UserTypeId))
            UserTypeId = request.UserTypeId;

        if (!Username.Equals(request.Username))
            Username = request.Username;

        if (!FirstName.Equals(request.FirstName))
            FirstName = request.FirstName;

        if (!LastName.Equals(request.LastName))
            LastName = request.LastName;

        if (!Prefix?.Equals(request.Prefix) ?? false)
            Prefix = request.Prefix;

        if (!Suffix?.Equals(request.Suffix) ?? false)
            Suffix = request.Suffix;

        if (!DateOfBirth?.Equals(request.DateOfBirth) ?? false)
            DateOfBirth = request.DateOfBirth;

        if (hash == await GetHash()) return;

        ModifiedDate = DateTime.UtcNow;

        if (userId.GetValueOrDefault() == Guid.Empty)
            ModifiedBy = userId;

        AcceptChanges();
    }

    /// <summary>
    /// </summary>
    /// <param name="request"></param>
    /// <param name="userId"></param>
    public async Task ApplyChanges(RegisterCommunityAdminRequest request, Guid? userId)
    {
        var hash = await GetHash();

        if (!UserTypeId.Equals(UserTypes.GetKey(Strings.UserType_CommunityAdministrator)))
            UserTypeId = UserTypes.GetKey(Strings.UserType_CommunityAdministrator);

        if (!Username.Equals(request.Username))
            Username = request.Username;

        if (!FirstName.Equals(request.FirstName))
            FirstName = request.FirstName;

        if (!LastName.Equals(request.LastName))
            LastName = request.LastName;

        if (!Prefix?.Equals(request.Prefix) ?? false)
            Prefix = request.Prefix;

        if (!Suffix?.Equals(request.Suffix) ?? false)
            Suffix = request.Suffix;

        if (!DateOfBirth?.Equals(request.DateOfBirth) ?? false)
            DateOfBirth = request.DateOfBirth;

        if (hash == await GetHash()) return;

        ModifiedDate = DateTime.UtcNow;

        if (userId.GetValueOrDefault() == Guid.Empty)
            ModifiedBy = userId;

        AcceptChanges();
    }
}