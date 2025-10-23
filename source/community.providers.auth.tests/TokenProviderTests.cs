using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using community.common.Definitions;
using community.common.Exceptions;
using community.models.Responses;
using community.models.Responses.Authentication;
using community.providers.auth.Implementation;
using community.providers.auth.Interfaces;
using community.tests.common;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.IdentityModel.Tokens;
using Moq;

namespace community.providers.auth.tests;

/// <summary>
///     Testing the token provider, ensuring tokens are created appropriately.
/// </summary>
public class TokenProviderTests : BaseTest
{
    private readonly ITokenProvider _tokenProvider;

    /// <summary>
    ///     Initialize the provider.
    /// </summary>
    public TokenProviderTests()
    {
        _tokenProvider = new TokenProvider(MockOptions.Object, MockHttpContextAccessor.Object,
            NullLogger<TokenProvider>.Instance);
    }

    /// <summary>
    ///     Generate Refresh Token stub test
    /// </summary>
    [Fact]
    [Trait("TestCategory", "Unit")]
    public void GenerateRefreshToken_ShouldSucceed()
    {
        var refreshToken = _tokenProvider.GenerateRefreshToken(It.IsAny<Guid>());
        Assert.NotNull(refreshToken);
    }

    /// <summary>
    ///     Generate Access Token stub test
    /// </summary>
    [Fact]
    [Trait("TestCategory", "Unit")]
    public void GenerateAccessToken_ValidateToken_ShouldSucceed()
    {
        var user = new UserResponse
        {
            Id = Guid.NewGuid(),
            FirstName = nameof(UserResponse.FirstName),
            LastName = nameof(UserResponse.LastName),
            Username = "jason.shepard@protonmail.com",
            UserTypeId = UserTypes.Values
                .Where(ut => ut.Value == "Site Administrator")
                .Select(ut => ut.Key)
                .FirstOrDefault(),
            Claims = new List<ClaimResponse>
            {
                new() { Name = "profile.View" },
                new() { Name = "profile.Edit" },
                new() { Name = "community.PostMessage" },
                new() { Name = "community.PostNotification" },
                new() { Name = "community.VerifyMember" }
            }
        };

        var accessToken = _tokenProvider.GenerateAccessToken(user);
        Assert.NotNull(accessToken);
        Assert.NotEmpty(accessToken);

        var securityToken = _tokenProvider.ValidateToken(accessToken) as JwtSecurityToken;
        Assert.NotNull(securityToken);
        Assert.Equal(MockSettings.Issuer, securityToken.Issuer);
        Assert.Equal(SecurityAlgorithms.HmacSha256, securityToken.SignatureAlgorithm);
        Assert.Contains(securityToken.Claims,
            c => c.Type.Equals(nameof(ClaimTypes.Role), StringComparison.CurrentCultureIgnoreCase) &&
                 c.Value == user.UserType);
        Assert.Contains(securityToken.Claims,
            c => c.Type.Equals(CommunityClaims.UserId, StringComparison.CurrentCultureIgnoreCase) &&
                 c.Value == user.Id.ToString());
    }

    /// <summary>
    ///     Validating an expired token
    /// </summary>
    [Fact]
    [Trait("TestCategory", "Unit")]
    public void ValidateToken_ExpiredToken_ShouldSucceed()
    {
        var expiredToken =
            "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJnaXZlbl9uYW1lIjoiRmlyc3ROYW1lIiwiZmFtaWx5X25hbWUiOiJMYXN0TmFtZSIsImp0aSI6IjQ0UmhpbmtFc09iU2RUa1ZTdXZCanl5akJ3cFFDME1Yb2pFNUdQZ0xtclVkS01pOXBMSG9QU0NDc3dzN0xVeUYiLCJpc3MiOiJJc3N1ZXIuY29tIiwiYXVkIjoiQXVkaWVuY2UuY29tIiwic3ViIjoiamFzb24uc2hlcGFyZEBwcm90b25tYWlsLmNvbSIsImNjaSI6IiIsInVpZCI6IjY2NGZlOThlLWM1MWQtNGRiNC1iZWQzLTQyZGQzNGZlMzAwNSIsInJvbGUiOiJTaXRlIEFkbWluaXN0cmF0b3IiLCJwZXJtaXNzaW9uIjpbInByb2ZpbGUuVmlldyIsInByb2ZpbGUuRWRpdCIsImNvbW11bml0eS5Qb3N0TWVzc2FnZSIsImNvbW11bml0eS5Qb3N0Tm90aWZpY2F0aW9uIiwiY29tbXVuaXR5LlZlcmlmeU1lbWJlciJdLCJuYmYiOjE3NjExODk0NTEsImV4cCI6MTc2MTE5MTI1MSwiaWF0IjoxNzYxMTg5NDUxfQ.WcZl9USAV40qHW-LzJ5i8y3JSjfG6HoVAdAfDB_zj7E";
        Assert.Throws<BusinessRuleException>(() => _tokenProvider.ValidateToken(expiredToken));
    }
}