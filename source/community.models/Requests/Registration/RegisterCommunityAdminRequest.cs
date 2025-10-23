using community.common.Definitions;
using community.common.Exceptions;
using community.common.Extensions;
using community.common.Utilities;
using community.data.entities;
using community.models.Abstract;

namespace community.models.Requests.Registration;

/// <summary>
///     Encapsulates all the required and optional properties required during the registration process.
/// </summary>
/// <param name="Password">Optional, the password the user has chosen.</param>
/// <param name="Prefix">The user's prefix, (eg: Mr. Mrs.)</param>
/// <param name="FirstName">Users firstname</param>
/// <param name="LastName">UserModel's lastname</param>
/// <param name="Suffix">Optional, suffix (eg: Jr, Sr)</param>
/// <param name="DateOfBirth">The user's date of birth</param>
/// <param name="Address">Optional address information for the community admin</param>
/// <param name="PhoneNumber">Optional phone number to register for users who use an email for usernamme.</param>
public record RegisterCommunityAdminRequest(
    string? Password,
    string? Prefix,
    string FirstName,
    string LastName,
    string? Suffix,
    DateOnly? DateOfBirth,
    RegisterAddressRequest? Address,
    RegisterContactRequest? PhoneNumber)
    : VerifiableUserNameRecord
{
    /// <summary>
    ///     Converts the immutable record to a user object for save purposes
    /// </summary>
    /// <returns></returns>
    public User ToEntity(Guid? lastCommunityId = null)
    {
        return new User
        {
            UserTypeId = UserTypes.GetKey(Strings.UserType_CommunityAdministrator),
            LastCommunityId = lastCommunityId,
            Username = Username.IsValidUsPhoneNumber() ? Username.FormatUsPhoneNumber() : Username,
            Password = Password != null ? EncryptionHelper.Encrypt(Password) : null,
            Prefix = Prefix,
            FirstName = FirstName,
            LastName = LastName,
            Suffix = Suffix,
            DateOfBirth = DateOfBirth.ToEncryptedString()
        };
    }

    /// <summary>
    /// </summary>
    /// <param name="validationException"></param>
    /// <exception cref="ValidationException"></exception>
    public override void Validate(ValidationException? validationException = null)
    {
        var shouldThrow = validationException == null;

        validationException ??= new ValidationException(ValidationMessages.ValidationErrors);

        if (string.IsNullOrEmpty(FirstName))
            validationException.AddError(nameof(FirstName), ValidationMessages.FirstNameRequired);

        if (FirstName.Length < Integers.ValidationMinimumNameLength)
            validationException.AddError(nameof(FirstName),
                string.Format(ValidationMessages.NameLength, Integers.ValidationMinimumNameLength));

        if (string.IsNullOrEmpty(LastName))
            validationException.AddError(nameof(LastName), ValidationMessages.LastNameRequired);

        if (LastName.Length < Integers.ValidationMinimumNameLength)
            validationException.AddError(nameof(LastName),
                string.Format(ValidationMessages.NameLength, Integers.ValidationMinimumNameLength));

        Address?.Validate(validationException);
        PhoneNumber?.Validate(validationException);


        base.Validate(validationException);

        // if any validation errors have been recorded, throw the exception.
        if (validationException.Errors.Any() && shouldThrow)
            throw validationException;
    }

    /// <summary>
    ///     Adds the user id to child objects.
    /// </summary>
    /// <param name="id"></param>
    public void SetId(Guid id)
    {
        if (Address != null) Address.UserId = id;
        if (PhoneNumber != null) PhoneNumber.UserId = id;
    }
}