using Microsoft.Extensions.Configuration;

[assembly: InternalsVisibleTo("community.auth.api.tests")]
[assembly: InternalsVisibleTo("community.providers.auth.tests")]
[assembly: InternalsVisibleTo("community.providers.common.tests")]

namespace community.tests.common;

/// <summary>
/// </summary>
[ExcludeFromCodeCoverage]
public abstract class BaseTest
{
    /// <summary>
    /// </summary>
    protected static Guid CorrelationKey = Guid.NewGuid();

    /// <summary>
    /// </summary>
    protected readonly Mock<HttpContext> MockHttpContext = new();

    /// <summary>
    /// </summary>
    protected readonly Mock<IHttpContextAccessor> MockHttpContextAccessor = new();

    /// <summary>
    /// </summary>
    protected readonly Mock<IOptions<JwtSettings>> MockOptions = new();

    /// <summary>
    /// </summary>
    protected readonly JwtSettings MockSettings = new()
    {
        Audience = $"{nameof(JwtSettings.Audience)}.com",
        Issuer = $"{nameof(JwtSettings.Issuer)}.com",
        Secret = EncryptionHelper.GenerateSalt(),
        TokenLifetime = new TimeSpan(0, 30, 0),
        RefreshTokenLifetime = new TimeSpan(2, 0, 0)
    };

    /// <summary>
    /// </summary>
    protected BaseTest()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddUserSecrets<BaseTest>()
            .AddJsonFile("appsettings.json", false, true)
            .Build();
        
        ConfigurationHelper.Initialize(configuration);
        
        MockOptions
            .Setup(j => j.Value)
            .Returns(MockSettings);

        MockHttpContext
            .Setup(c => c.Items)
            .Returns(new Dictionary<object, object?>
            {
                { "CorrelationId", $"{CorrelationKey}" }
            });

        var claims = new List<Claim>
        {
            new(CommunityClaims.CurrentCommunityId, "0196a8b6-6994-750e-9c91-898c2ae7d9d0"),
            new(CommunityClaims.UserId, "0196a8b6-6d53-737c-9c9b-94b35eea2265")
        };

        var claimsIdentity = new ClaimsIdentity(claims);
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        MockHttpContext.Setup(c => c.Items).Returns(new Dictionary<object, object?>
        {
            { Strings.Header_CorrelationId, $"{Guid.NewGuid()}" }
        });

        MockHttpContext.Setup(c => c.User).Returns(claimsPrincipal);
        MockHttpContextAccessor.Setup(c => c.HttpContext).Returns(MockHttpContext.Object);
    }
}