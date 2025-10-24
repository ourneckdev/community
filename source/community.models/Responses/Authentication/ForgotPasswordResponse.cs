namespace community.models.Responses.Authentication;

/// <summary>
///     response object when initiating a forgot password flow.
/// </summary>
/// <param name="LoginCode"></param>
public record ForgotPasswordResponse(string LoginCode);