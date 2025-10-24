namespace community.models.Requests.Authentication;

/// <summary>
///     Accepts the username that initiates the forgot password request.
/// </summary>
/// <param name="Username"></param>
public record ForgotPasswordRequest(string Username);