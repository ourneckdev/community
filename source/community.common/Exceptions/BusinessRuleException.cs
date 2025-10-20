namespace community.common.Exceptions;

/// <summary>
///     Defines a login exception which implements the <see cref="CommunityException" />.
/// </summary>
/// <param name="message">The error message for the condition</param>
public class BusinessRuleException(string message) : CommunityException(message) { }