using community.common.Definitions;
using community.common.Exceptions;
using community.common.Extensions;
using community.models.Requests;

namespace community.models.Abstract;

/// <summary>
/// Validates the implementing record for valid email or phone number
/// </summary>
public abstract record VerifiableUserNameRecord : IRequiresValidation
{
    /// <summary>
    /// Requires implementation of a username to require validation.
    /// </summary>
    public string Username { get; init; } = null!;
    
    
    /// <inheritdoc cref="IRequiresValidation.Validate" />
    public virtual void Validate(ValidationException? exception = null)
    {
        var shouldThrow = exception == null;
        
        exception ??= new ValidationException();
        
        if (!IsValidEmailAddress() && !IsValidPhoneNumber())
            exception.AddError(nameof(Username), ValidationMessages.UserNameInvalid);
        
        if (exception.Errors.Any() && shouldThrow)
            throw exception;
    }
    
    
    /// <summary>
    /// Verifies the username is a valid phonenumber
    /// </summary>
    /// <returns></returns>
    private bool IsValidPhoneNumber() => Username.IsValidUsPhoneNumber();
    
    /// <summary>
    /// Verifieds if the username is a valid email address.
    /// </summary>
    /// <returns></returns>
    private bool IsValidEmailAddress() => Username.IsValidEmailAddress();
};