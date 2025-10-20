using community.common.Definitions;
using community.common.Enumerations;
using community.common.Exceptions;
using community.common.Extensions;
using community.data.entities;

namespace community.models.Requests.Registration;

/// <summary>
/// Defines the required properties necessary for collecting contact information during the registration process. 
/// </summary>
/// <param name="ContactMethodId"></param>
/// <param name="Value"></param>
public record RegisterContactRequest(Guid ContactMethodId, string Value) 
    : IRequiresValidation
{
    /// <summary>
    /// 
    /// </summary>
    public Guid CommunityId { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    public Guid? UserId { get; set; }
    
    /// <summary>
    /// Maps an incoming request object to a database entity.
    /// </summary>
    /// <returns></returns>
    public Contact ToContact(EntityType type)
    {
        return new Contact
        {
            CommunityId = CommunityId,
            UserId = type == EntityType.User ? UserId.GetValueOrDefault() : null,
            ContactMethodId = ContactMethodId,
            Value = Value.IsValidPhoneNumber() ? Value.FormatPhoneNumber() : Value,
            EntityType = type
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
        
        if(!Value.IsValidEmailAddress() && !Value.IsValidPhoneNumber())
            exception.AddError(nameof(Value), ValidationMessages.ContactMethodInvalid);
        
        if(exception.Errors.Any() && shouldThrow)
            throw exception;
    }
}
    