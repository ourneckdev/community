using community.common.Interfaces;
using community.data.entities;
using community.models.Responses;
using community.models.Responses.Authentication;
using Microsoft.IdentityModel.Tokens;

namespace community.providers.auth.Interfaces;

/// <summary>
///     Provider responsible for generating and revoking access and refresh tokens.
/// </summary>
public interface ITokenProvider : IProvider
{
    /// <summary>
    ///     Generates a JWT Access Token comprising the user information and claims needed for authorization.
    /// </summary>
    /// <param name="user">The user object used to populate the token</param>
    /// <returns>A populated JWT Access Token</returns>
    string GenerateAccessToken(UserResponse user);

    /// <summary>
    ///     Validates a bearer token string against the correct token validation parameters
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    SecurityToken ValidateToken(string token);

    /// <summary>
    ///     Generates a cryptographically secure string to use as the refresh token.
    /// </summary>
    /// <returns>Returns a base-64 representation of a 32 Byte cryptographically secure random string.</returns>
    string GenerateRefreshToken(Guid userId);


    /// <summary>
    ///     Looks up a user by id, returns a hydrated LoginResponse containing the Access and Refresh Tokens, and user
    ///     information.
    /// </summary>
    /// <param name="user">The user object the access and refresh tokens are being generated against.w</param>
    /// <returns></returns>
    LoginResponse GenerateTokens(User user);
}