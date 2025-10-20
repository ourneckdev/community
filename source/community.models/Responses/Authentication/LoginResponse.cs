using community.common.BaseClasses;

namespace community.models.Responses.Authentication;

/// <summary>
///     The JWT generated during login.
/// </summary>
/// <param name="AccessToken">The Access token containing claims used for authorizing users.</param>
/// <param name="RefreshToken">The refresh token used for renewing the AccessToken during active sessions.</param>
/// <param name="User">The user data returned during a login operation.</param>
public record LoginResponse(
    string AccessToken,
    string RefreshToken,
    UserResponse User) : BaseRecord;