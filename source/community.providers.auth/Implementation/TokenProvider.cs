using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using community.common.AppSettings;
using community.common.BaseClasses;
using community.common.Definitions;
using community.common.Extensions;
using community.common.Utilities;
using community.data.entities;
using community.models.Responses;
using community.models.Responses.Authentication;
using community.providers.auth.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

[assembly: InternalsVisibleTo("community.auth.providers.Tests")]

namespace community.providers.auth.Implementation;

/// <summary>
/// Implements ITokenProvider.  Extends methods required to generate Access and Refresh Tokens.
/// </summary>
/// <param name="jwtSettings"></param>
/// <param name="contextAccessor"></param>
public class TokenProvider(IOptions<JwtSettings> jwtSettings, IHttpContextAccessor contextAccessor) 
    : BaseProvider(contextAccessor), ITokenProvider
{
    private readonly JwtSecurityTokenHandler _tokenHandler = new();

    /// <inheritdoc cref="ITokenProvider.GenerateAccessToken"/>
    public string GenerateAccessToken(UserResponse user)
    {
        var timeSpan = jwtSettings.Value.TokenLifetime;

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.GivenName, user.FirstName),
            new(JwtRegisteredClaimNames.FamilyName, user.LastName),
            new(JwtRegisteredClaimNames.Jti, EncryptionHelper.Generate(48)),
            new(JwtRegisteredClaimNames.Iss, jwtSettings.Value.Issuer),
            new(JwtRegisteredClaimNames.Aud, jwtSettings.Value.Audience),
            new(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToUnixTime(), ClaimValueTypes.Integer64),
            new(JwtRegisteredClaimNames.Exp, DateTime.UtcNow.AddMinutes(timeSpan.Minutes).ToUnixTime(), ClaimValueTypes.Integer64),

            new(CommunityClaims.CurrentCommunityId, $"{user.LastCommunityId}"), // current community ID
            new(CommunityClaims.UserId, $"{user.Id}"), // the user's ID
            new(ClaimTypes.Role, user.UserType)
        };
        
        claims.AddRange(user.Claims.Select(c => new Claim("permission", c.Name)));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.Add(jwtSettings.Value.TokenLifetime),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Value.Secret)),
                SecurityAlgorithms.HmacSha256)
        };

        var token = _tokenHandler.CreateToken(tokenDescriptor);
        var accessToken = _tokenHandler.WriteToken(token);
        return accessToken;
    }

    /// <inheritdoc cref="ITokenProvider.GenerateRefreshToken"/>
    public string GenerateRefreshToken(Guid userId)
    {
        return EncryptionHelper.Generate();
    }

    /// <inheritdoc cref="ITokenProvider.ValidateToken"/>
    public SecurityToken ValidateToken(string token)
    {
        _tokenHandler
            .ValidateToken(token,
                new TokenValidationParameters
                {
                    ClockSkew = TimeSpan.FromMinutes(5),
                    RequireSignedTokens = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Value.Secret)),
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings.Value.Issuer,
                    ValidateAudience = true,
                    ValidAudience = jwtSettings.Value.Audience
                },
                out var validatedToken);
        return validatedToken;
    }

    /// <inheritdoc />
    public LoginResponse GenerateTokens(User user)
    {
        var accessToken = GenerateAccessToken(user);
        var refreshToken = GenerateRefreshToken(user.Id);
        return new LoginResponse(accessToken, refreshToken, user) { CorrelationId = CorrelationId };
    }
}