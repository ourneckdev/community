using community.common.Exceptions;
using community.common.Interfaces;
using community.models.Requests.Authentication;
using community.models.Responses;
using community.models.Responses.Authentication;
using community.models.Responses.Base;
using ForgotPasswordRequest = community.models.Requests.Authentication.ForgotPasswordRequest;

namespace community.providers.auth.Interfaces;

/// <summary>
///     Defines the available methods for enabling a user to login
/// </summary>
public interface IAuthenticationProvider : IProvider
{
    /// <summary>
    ///     The first step to logging in is to request a login code, which is initiated when a user enters
    ///     the email address or phone number they registered with.
    /// </summary>
    /// <param name="username">The initial email or password the user joined with</param>
    /// <returns>a generated 6 digit login code</returns>
    Task<SingleResponse<LoginCodeResponse>> RequestLoginCodeAsync(string username);

    /// <summary>
    ///     Contains the user's username and generated login code to verify the user's account.
    /// </summary>
    /// <param name="withCodeRequest">The username and security code required or logging in.</param>
    /// <returns>A JWT AccessToken, RefreshToken and the <see cref="UserResponse" /></returns>
    /// <exception cref="BusinessRuleException">An exception thrown if the user isn't found or their password is incorrect.</exception>
    Task<SingleResponse<LoginResponse>> LoginAsync(LoginWithCodeRequest withCodeRequest);

    /// <summary>
    ///     Logs a user in with a password as a backup to logging in with email/sms
    /// </summary>
    /// <param name="withCodeRequest">The username and password required for logging in</param>
    /// <returns>A JWT AccessToken, RefreshToken and the <see cref="UserResponse" /></returns>
    /// <exception cref="BusinessRuleException">An exception thrown if the user isn't found or their password is incorrect.</exception>
    Task<SingleResponse<LoginResponse>> LoginWithPasswordAsync(LoginWithPasswordRequest withCodeRequest);

    /// <summary>
    ///     For users who have opted to log in with a password, we need to expose functionality that allows them
    ///     to reset their password.
    /// </summary>
    /// <param name="forgotPasswordRequest">
    ///     Immutable record containing the required properties necessary for initiating a
    ///     forgot password flow.
    /// </param>
    /// <returns>a response indicating if the process was executed successfully and any relevant messaging.</returns>
    Task<SingleResponse<ForgotPasswordResponse>> ForgotPasswordAsync(ForgotPasswordRequest forgotPasswordRequest);

    /// <summary>
    /// Generates a one time code verifier and code challenge 
    /// </summary>
    /// <returns></returns>
    SingleResponse<(string Challenge, string Verifier)> GenerateProof();
}