namespace community.auth.api.tests;

/// <summary>
/// Instantiates a test web server for executing Integration tests against the Auth api.
/// </summary>
[ExcludeFromCodeCoverage]
public class TestApplicationFactory : WebApplicationFactory<Program>
{
    private readonly JwtSecurityTokenHandler _tokenHandler = new();

    /// <summary>
    /// Handles token validation, but need to not use this since it's already handled in the provider.
    /// TODO: get provider working for validation.
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    public SecurityToken ValidateToken(string token)
    {
        var jwtSettings = Services.GetRequiredService<IOptions<JwtSettings>>().Value;
        _tokenHandler
            .ValidateToken(token,
                new TokenValidationParameters
                {
                    ClockSkew = TimeSpan.FromMinutes(5),
                    RequireSignedTokens = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidateAudience = true,
                    ValidAudience = jwtSettings.Audience
                },
                out var validatedToken);
        return validatedToken;
    }
}