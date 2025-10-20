namespace community.common.Exceptions;

/// <summary>
///     Defines a NotFoundException that implements a <see cref="CommunityException" />
/// </summary>
/// <param name="message">Initializes the not found error code.</param>
public class NotFoundException(string message) : CommunityException(message)
{
}