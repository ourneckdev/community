namespace community.models.Responses.Authentication;

/// <summary>
///     Encapsulates the response for requesting a login code.
/// </summary>
/// <param name="Code">The generated code.</param>
public record LoginCodeResponse(string Code);