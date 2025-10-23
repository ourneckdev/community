using System.IdentityModel.Tokens.Jwt;
using System.Text;
using community.common.AppSettings;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace community.auth.api.tests;

public class TestApplicationFactory : WebApplicationFactory<Program>
{
    private readonly JwtSecurityTokenHandler _tokenHandler = new();

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