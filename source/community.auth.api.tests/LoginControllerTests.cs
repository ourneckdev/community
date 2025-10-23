using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text;
using community.models.Responses;
using community.models.Responses.Authentication;
using community.models.Responses.Base;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace community.auth.api.tests;

public class LoginControllerTests
{
    private readonly TestApplicationFactory _factory = new();
    private readonly HttpClient _httpClient;
    private readonly string _testPassword = "BlahBlahBlah98!";
    private readonly string _testUser = "ourneckofthewoods@proton.me";

    //private readonly ITokenProvider? _tokenProvider;

    public LoginControllerTests()
    {
        _httpClient = _factory.CreateClient();
        _httpClient.BaseAddress = new Uri("https://localhost:7279/login/");
    }

    [Trait("Category", "Integration")]
    [Fact]
    public async Task LoginWithPassword_ShouldSucceed()
    {
        var loginRequest = new
        {
            username = _testUser,
            password = _testPassword
        };

        var postBody = new StringContent(JsonSerializer.Serialize(loginRequest),
            Encoding.UTF8,
            "application/json");
        var response = await _httpClient.PostAsync("withpassword", postBody);
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        var itemResponse = JsonConvert.DeserializeObject<SingleResponse<LoginResponse>>(content);
        Assert.NotNull(itemResponse);
        Assert.NotNull(itemResponse.Item);
        Assert.IsType<LoginResponse>(itemResponse.Item);
        Assert.NotNull(itemResponse.Item.AccessToken);

        var jwt = itemResponse.Item.AccessToken;
        var token = _factory.ValidateToken(jwt) as JwtSecurityToken;
        Assert.NotNull(token);
        Assert.Equal(_testUser, token.Subject);
        Assert.True(token.IssuedAt < DateTime.UtcNow);
        Assert.True(token.ValidTo > DateTime.UtcNow);

        Assert.IsType<UserResponse>(itemResponse.Item.User);
        Assert.NotNull(itemResponse.Item.User);
        Assert.NotNull(itemResponse.Item.User.DateOfBirth);
        Assert.Equal(new DateOnly(2001, 9, 11), itemResponse.Item.User.DateOfBirth);
        Assert.Equal(_testUser, itemResponse.Item.User.Username);
    }
}