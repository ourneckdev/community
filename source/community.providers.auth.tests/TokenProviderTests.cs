using community.common.AppSettings;
using community.common.Definitions;
using community.common.Utilities;
using community.models.Responses;
using community.models.Responses.Authentication;
using community.providers.auth.Implementation;
using community.providers.auth.Interfaces;
using community.tests.common;
using Microsoft.Extensions.Options;
using Moq;

namespace community.providers.auth.tests;

public class TokenProviderTests : BaseTest
{
    private readonly Mock<IOptions<JwtSettings>> _mockOptions = new();
    private readonly ITokenProvider _tokenProvider;

    public TokenProviderTests()
    {
        _tokenProvider = new TokenProvider(_mockOptions.Object, _mockHttpContextAccessor.Object);
    }

    [Fact]
    public void GenerateRefreshToken_ShouldSucceed()
    {
        var refreshToken = _tokenProvider.GenerateRefreshToken(It.IsAny<Guid>());
        Assert.NotNull(refreshToken);
    }

    [Fact]
    public void GenerateAccessToken_ValidateToken_ShouldSucceed()
    {
        var mockSettings = new JwtSettings
        {
            Audience = $"{nameof(JwtSettings.Audience)}.com",
            Issuer = $"{nameof(JwtSettings.Issuer)}.com",
            Secret = EncryptionHelper.Generate(),
            TokenLifetime = new TimeSpan(0, 30, 0),
            RefreshTokenLifetime = new TimeSpan(2, 0, 0)
        };
        _mockOptions
            .Setup(j => j.Value)
            .Returns(mockSettings);

        var user = new UserResponse
        {
            Id = Guid.NewGuid(),
            FirstName = nameof(UserResponse.FirstName),
            LastName = nameof(UserResponse.LastName),
            Username = "jason.shepard@protonmail.com",
            UserTypeId = UserTypes.Values.Where(ut => ut.Value == "Site Administrator").Select(ut => ut.Key)
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

        var securityToken = _tokenProvider.ValidateToken(accessToken);
        Assert.Equal(mockSettings.Issuer, securityToken.Issuer);
    }
}