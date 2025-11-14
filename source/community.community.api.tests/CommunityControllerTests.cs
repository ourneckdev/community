using community.auth.api.tests;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Text;
using community.models.Responses.Authentication;

namespace community.community.api.tests;

/// <summary>
///     Integration tests covering the Community controller.
/// </summary>
public class CommunityControllerTests
{
    private readonly TestApplicationFactory<Program> _factory = new();
    private readonly AuthApplicationFactory _authFactory = new();
    private readonly HttpClient _httpClient;
    private readonly HttpClient _authClient;
    private readonly string _testPassword = "BlahBlahBlah98!";
    private readonly string _testUser = "ourneckofthewoods@proton.me";
    private const string BaseAddress = "http://localhost:7063/community/";
    private const string AuthBaseAddress = "https://localhost:7279/login/";
    private readonly Guid _testCommunityId = Guid.Parse("019a7dee-2dde-7d74-8f62-7d7837a38c9d");
    
    private readonly JwtSecurityTokenHandler _tokenHandler = new();
    private string? _accessToken;

    /// <summary>
    /// Basic name search 
    /// </summary>
    [Fact, Trait("Category", "Integration")]
    public async Task SearchCommunities_NameOnlySearch_ShouldSucceed()
    {
        var response = await _httpClient.GetAsync("search?name=test+community");
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        var content = await response.Content.ReadAsStringAsync();
        var communityResponse = JsonConvert.DeserializeObject<MultiResponse<CommunityResponse?>>(content);
        Assert.NotNull(communityResponse);
        Assert.NotNull(communityResponse.Items);
        Assert.IsType<List<CommunityResponse>>(communityResponse.Items);
        Assert.Single(communityResponse.Items);
        
        Assert.Equal(_testCommunityId, communityResponse.Items.Single()?.Id);
    }
    
    /// <summary>
    /// Basic name search 
    /// </summary>
    [Fact, Trait("Category", "Integration")]
    public async Task SearchCommunities_ZipcodeSearch_ShouldSucceed()
    {
        var response = await _httpClient.GetAsync("search?postalcode=80102");
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        var content = await response.Content.ReadAsStringAsync();
        var communityResponse = JsonConvert.DeserializeObject<MultiResponse<CommunityResponse?>>(content);
        Assert.NotNull(communityResponse);
        Assert.NotNull(communityResponse.Items);
        Assert.IsType<List<CommunityResponse>>(communityResponse.Items);
        Assert.Single(communityResponse.Items);
        
        Assert.Equal(_testCommunityId, communityResponse.Items.Single()?.Id);
    }
    
    /// <summary>
    /// Basic name search 
    /// </summary>
    [Fact, Trait("Category", "Integration")]
    public async Task SearchCommunities_StateCodeSearch_ShouldSucceed()
    {
        var response = await _httpClient.GetAsync("search?stateCode=CO");
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        var content = await response.Content.ReadAsStringAsync();
        var communityResponse = JsonConvert.DeserializeObject<MultiResponse<CommunityResponse?>>(content);
        Assert.NotNull(communityResponse);
        Assert.NotNull(communityResponse.Items);
        Assert.IsType<List<CommunityResponse>>(communityResponse.Items);
        Assert.True(communityResponse.Items.Count() > 1);
        
        Assert.NotNull(communityResponse.Items.FirstOrDefault(i => i?.Id == _testCommunityId));
    }
    
    /// <summary>
    /// Basic name search 
    /// </summary>
    [Fact, Trait("Category", "Integration")]
    public async Task SearchCommunities_AddressLine1Search_ShouldSucceed()
    {
        var response = await _httpClient.GetAsync("search?addressLine1=1234+Easy+St");
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        var content = await response.Content.ReadAsStringAsync();
        var communityResponse = JsonConvert.DeserializeObject<MultiResponse<CommunityResponse?>>(content);
        Assert.NotNull(communityResponse);
        Assert.NotNull(communityResponse.Items);
        Assert.IsType<List<CommunityResponse>>(communityResponse.Items);
        Assert.Single(communityResponse.Items);
        
        Assert.NotNull(communityResponse.Items.FirstOrDefault(i => i?.Id == _testCommunityId));
    }

    /// <summary>
    /// Searches for the test community then uses the authorized client to retrieve the details about that community.
    /// </summary>
    [Fact, Trait("Category", "Integration")]
    public async Task GetCommunity_ById_ShouldSucceed()
    {
        var response = await _httpClient.GetAsync("search?name=test+community");
        var content = await response.Content.ReadAsStringAsync();
        var communityResponse = JsonConvert.DeserializeObject<MultiResponse<CommunityResponse?>>(content);
        Assert.NotNull(communityResponse);

        if (TokenIsEmptyOrExpired()) await LoginAsync();
        
        _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _accessToken);
        var testResponse = await _httpClient.GetAsync($"{communityResponse.Items.Single()?.Id}");
        Assert.Equal(HttpStatusCode.OK, testResponse.StatusCode);
        Assert.NotNull(testResponse);
        
        var testResponseContent = await testResponse.Content.ReadAsStringAsync();
        Assert.NotEmpty(testResponseContent);
        var community = JsonConvert.DeserializeObject<SingleResponse<CommunityResponse>>(testResponseContent);
        Assert.NotNull(community);
        Assert.NotNull(community.Item);
        Assert.Equal(communityResponse.Items.Single()?.Id, community.Item.Id);
    }

    
    /// <summary>
    /// Initializes the integration tests against the community set of endpoints.
    /// </summary>
    public CommunityControllerTests()
    {
        _httpClient = _factory.CreateClient();
        _httpClient.BaseAddress = new Uri(BaseAddress);
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        
        _authClient = _authFactory.CreateClient();
        _authClient.BaseAddress = new Uri(AuthBaseAddress);
    }
    
    
    private async Task LoginAsync()
    {
        var loginWithPasswordRequest = new
        {
            username = _testUser,
            password = _testPassword
        };
        
        var postBody = new StringContent(JsonConvert.SerializeObject(loginWithPasswordRequest), Encoding.UTF8, "application/json");
        using var response = await _authClient.PostAsync("withpassword", postBody);
        var content = await response.Content.ReadAsStringAsync();
        var itemResponse = JsonConvert.DeserializeObject<SingleResponse<LoginResponse>>(content);
        if(itemResponse == null) throw new AccessViolationException("Could not login with supplied credentials.");
        _accessToken = itemResponse.Item.AccessToken;
    }
    
    private bool TokenIsEmptyOrExpired()
    {
        if(_accessToken == null) return true;
        var validToken = _tokenHandler.ReadJwtToken(_accessToken);
        return validToken.ValidTo < DateTime.UtcNow;
    }
}