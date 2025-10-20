using System.Text.Json.Serialization;

namespace community.models.Requests.Authentication;

/// <summary>
///     The input properties required for logging a user in.
/// </summary>
/// <param name="Username">The phone number or email address the user registered with.</param>
/// <param name="Password">The optional password to supply, if bypassing login with code</param>
public record LoginWithPasswordRequest(
    [property: JsonPropertyName("username")]
    string Username,
    
    [property: JsonPropertyName("password")]
    string? Password);