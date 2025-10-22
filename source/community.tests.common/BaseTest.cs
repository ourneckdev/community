using System.Security.Claims;
using community.common.Definitions;
using Microsoft.AspNetCore.Http;
using Moq;

namespace community.tests.common;

public abstract class BaseTest
{
    protected readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor = new();
    protected readonly Mock<HttpContext> _mockHttpContext = new();
    public static Guid CorrelationKey = Guid.NewGuid();

    public BaseTest()
    {
        _mockHttpContext
            .Setup(c => c.Items)
            .Returns(new Dictionary<object, object?>
            {
                { "CorrelationId", $"{CorrelationKey}" }
            });

        var claims = new List<Claim>
        {
            new Claim(CommunityClaims.CurrentCommunityId, "0196a8b6-6994-750e-9c91-898c2ae7d9d0"),
            new Claim(CommunityClaims.UserId, "0196a8b6-6d53-737c-9c9b-94b35eea2265")
        };
        
        var claimsIdentity = new ClaimsIdentity(claims);
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
        
        _mockHttpContext.Setup(c => c.User).Returns(claimsPrincipal);
        _mockHttpContextAccessor.Setup(c => c.HttpContext).Returns(_mockHttpContext.Object);
    }
}