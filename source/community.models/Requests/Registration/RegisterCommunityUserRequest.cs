using community.common.Definitions;
using community.common.Exceptions;
using community.common.Utilities;
using community.data.entities;
using community.models.Abstract;

namespace community.models.Requests.Registration;

/// <summary>
/// Registers a user to an existing community.
/// </summary>
/// <param name="CommunityId">The id of the community the user is attmepting to join.</param>
/// <param name="Password">Optional, the password the user has chosen.</param>
/// <param name="Prefix">The user's prefix, (eg: Mr. Mrs.)</param>
/// <param name="FirstName">Users firstname</param>
/// <param name="LastName">UserModel's lastname</param>
/// <param name="Suffix">Optional, suffix (eg: Jr, Sr)</param>
/// <param name="DateOfBirth">The user's date of birth</param>
/// <param name="Address"></param>
/// <param name="ContactMethods"></param>
public record RegisterCommunityUserRequest(
    Guid CommunityId,
    string? Password,
    string? Prefix,
    string FirstName,
    string LastName,
    string? Suffix,
    DateOnly? DateOfBirth,
    RegisterAddressRequest? Address,
    IEnumerable<RegisterContactRequest>? ContactMethods) : VerifiableUserNameRecord
{
    /// <summary>
    /// Maps the request to a series of database objects to add.
    /// </summary>
    /// <returns></returns>
    public User ToUser()
    {
        return new User
        {
            UserTypeId = UserTypes.GetKey(Strings.UserType_CommunityMember),
            Username = Username,
            Password = Password != null ? EncryptionHelper.Encrypt(Password) : null,
            Prefix = Prefix,
            FirstName = FirstName,
            LastName = LastName,
            Suffix = Suffix,
            DateOfBirth = DateOfBirth.ToEncryptedString()
        };
    }

    /// <inheritdoc cref="IRequiresValidation.Validate" />
    public void Validate()
    {
        var validationException = new ValidationException(ValidationMessages.ValidationErrors);

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

        base.Validate(validationException);
        
        Address?.Validate(validationException);
        
        if(ContactMethods?.Any() ?? false)
            foreach(var contactMethod in ContactMethods)
                contactMethod.Validate(validationException);
        
        if(validationException.Errors.Any())
            throw validationException;
    }
}