using community.common.Exceptions;

namespace community.models.Requests;

/// <summary>
///     Requires requests to include a validation method.
/// </summary>
public interface IRequiresValidation
{
    /// <summary>
    ///     Validates the record for business logic.
    /// </summary>
    /// <param name="exception">The optional exception to append errors to.</param>
    void Validate(ValidationException? exception = null);
}