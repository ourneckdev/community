using community.common.Definitions;

namespace community.common.Exceptions;

/// <summary>
///     Defines a validation exception thrown during app operation.
/// </summary>
public class ValidationException(string message, IDictionary<string, string[]> errors) : CommunityException(message)
{
    /// <summary>
    ///     Instantiates a new ValidationException with message only.
    /// </summary>
    /// <param name="message"></param>
    /// <exception cref="NotImplementedException"></exception>
    public ValidationException(string message)
        : this(message, new Dictionary<string, string[]>())
    {
    }

    /// <summary>
    ///     Standard constructor
    /// </summary>
    public ValidationException()
        : this(ValidationMessages.ValidationErrors)
    {
    }

    /// <summary>
    ///     Summary of all validation errors.
    /// </summary>
    public IDictionary<string, string[]> Errors { get; } = errors;


    /// <summary>
    ///     Adds an error to the validation exception.
    /// </summary>
    /// <param name="key">The field being validated.</param>
    /// <param name="error">The error code to instantiate to or append the field with.</param>
    public void AddError(string key, string error)
    {
        if (Errors.ContainsKey(key))
        {
            Errors[key] = Errors[key].Append(error).ToArray();
            return;
        }

        Errors.Add(key, [error]);
    }
}