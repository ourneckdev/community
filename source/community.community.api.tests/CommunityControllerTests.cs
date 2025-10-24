using community.tests.common;
using communityApi = community.community.api;

namespace community.community.api.tests;

/// <summary>
/// Integration tests covering the Community controller.
/// </summary>
public class CommunityControllerTests
{
    private readonly TestApplicationFactory<communityApi.Program> _factory = new();
    
    /// <summary>
    /// 
    /// </summary>
    [Fact, Trait("Category", "Integration")]
    public void AddCommunity_MinimumInformation_ShouldSucceed()
    {
    }
}