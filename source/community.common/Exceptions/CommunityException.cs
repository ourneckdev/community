namespace community.common.Exceptions;

/// <summary>
///     Defines a base exception for all custom exceptions thrown which
///     allows logging of error codes for easier reference.
/// </summary>
/// <param name="message">The error message</param>
public class CommunityException(string message) : Exception(message)
{
}