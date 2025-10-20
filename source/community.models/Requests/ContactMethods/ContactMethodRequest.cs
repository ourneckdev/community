using community.common.Definitions;
using community.common.Exceptions;
using community.data.entities;
using community.models.Abstract;

namespace community.models.Requests.ContactMethods;

/// <summary>
/// Defines the required properties necessary for collecting contact information during the registration process. 
/// </summary>
/// <param name="ContactMethodId"></param>
/// <param name="Value"></param>
public record ContactMethodRequest(Guid ContactMethodId, string Value) 
    : BaseCommunityRecord, IRequiresValidation
{
    /// <summary>
    /// Maps an incoming request object to a database entity.
    /// </summary>]
    /// <param name="communityId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    public Contact ToContact(Guid communityId, Guid? userId = null)
    {
        return new Contact
        {
            CommunityId = communityId,
            UserId = userId.GetValueOrDefault(),
            ContactMethodId = ContactMethodId,
            Value = Value
        };
    }

    /// <inheritdoc cref="IRequiresValidation.Validate" />
    public void Validate(ValidationException? exception = null)
    {
        var shouldThrow = exception == null;
        
        exception ??= new ValidationException();
        
        if(ContactMethodId == Guid.Empty)
            exception.AddError(nameof(ContactMethodId), ValidationMessages.ContactMethodRequired);
        
        if(string.IsNullOrEmpty(Value))
            exception.AddError(nameof(Value), ValidationMessages.ContactMethodRequired);
        
        if(exception.Errors.Any() && shouldThrow)
            throw exception;
    }
}
    