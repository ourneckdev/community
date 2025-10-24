namespace community.models.Requests.Authentication;

/// <summary>
///     The input properties required for logging a user in.
/// </summary>
/// <param name="Username">The phone number or email address the user registered with.</param>
/// <param name="LoginCode">The login code generated when the user requests a login code</param>
public record LoginWithCodeRequest(string Username, string? LoginCode);