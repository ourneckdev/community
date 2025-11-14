using System.Security.Cryptography;
using System.Text;
using community.common.Extensions;

namespace community.common.Utilities;

/// <summary>
/// Initialization of the auth-flow challenge response.
/// </summary>
public static class Pkce
{
    /// <summary>
    /// Generates a Pkce challenge and verifier for auth flow
    /// </summary>
    /// <returns></returns>
    public static (string Challenge, string Verifier) Generate()
    {
        var verifier = EncryptionHelper.GenerateSalt().UrlEncodeForBase64();
        var challenge = EncryptionHelper.GenerateSalt().UrlEncodeForBase64();
        return(challenge, verifier);
    }

    /// <summary>
    /// Accepts a verify and generates a challenge for validating user actions.
    /// </summary>
    /// <param name="verifier"></param>
    /// <returns></returns>
    public static string GenerateChallenge(string verifier)
    {
        var buffer = Encoding.UTF8.GetBytes(verifier);
        return Convert.ToBase64String(SHA256.Create().ComputeHash(buffer)).UrlEncodeForBase64();
    }
}