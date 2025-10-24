using community.auth.api.Controllers;
using community.common.Definitions;
using Microsoft.AspNetCore.Mvc;

namespace community.auth.api.tests;

/// <summary>
///     Contains integration tests for exercising the functionality within the <see cref="LoginController" />
/// </summary>
[ExcludeFromCodeCoverage]
public class LoginControllerTests
{
    private readonly TestApplicationFactory _factory = new();
    private readonly HttpClient _httpClient;
    private readonly string _testPassword = "BlahBlahBlah98!";
    private readonly string _testUser = "ourneckofthewoods@proton.me";

    /// <summary>
    ///     Test initialization
    /// </summary>
    public LoginControllerTests()
    {
        _httpClient = _factory.CreateClient();
        _httpClient.BaseAddress = new Uri("https://localhost:7279/login/");
    }

    /// <summary>
    ///     Happy path login test
    /// </summary>
    [Fact]
    [Trait("Category", "Integration")]
    public async Task LoginWithPassword_TestUser_ShouldSucceed()
    {
        var loginRequest = new
        {
            username = _testUser,
            password = _testPassword
        };

        var postBody = new StringContent(JsonSerializer.Serialize(loginRequest), Encoding.UTF8, "application/json");
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

    /// <summary>
    ///     Happy path login test
    /// </summary>
    [Fact]
    [Trait("Category", "Integration")]
    public async Task LoginWithPassword_TestUserInvalidPassword_ShouldSucceed()
    {
        var loginRequest = new
        {
            username = _testUser,
            password = "123456"
        };

        var postBody = new StringContent(JsonSerializer.Serialize(loginRequest), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("withpassword", postBody);
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        var itemResponse = JsonConvert.DeserializeObject<ProblemDetails>(content);
        Assert.NotNull(itemResponse);
        Assert.Equal(ValidationMessages.ValidationErrors, itemResponse.Title);
        Assert.Equal(ErrorCodes.Login_IncorrectPassword, itemResponse.Detail);
    }

    /// <summary>
    ///     Happy path testing of logging in with a code.
    /// </summary>
    [Fact]
    [Trait("Category", "Integration")]
    public async Task LoginAsync_SendsValidLoginCode_ShouldSucceed()
    {
        var requestCodeResponse = await _httpClient.GetAsync($"{_testUser}/code");
        Assert.NotNull(requestCodeResponse);
        Assert.Equal(HttpStatusCode.OK, requestCodeResponse.StatusCode);

        var content = await requestCodeResponse.Content.ReadAsStringAsync();
        var code = JsonConvert.DeserializeObject<SingleResponse<LoginCodeResponse>>(content);
        Assert.NotNull(code);

        var loginWithCodeRequest = new
        {
            username = _testUser,
            loginCode = code.Item.Code
        };
        var loginRequest = new StringContent(JsonSerializer.Serialize(loginWithCodeRequest),
            Encoding.UTF8, "application/json");
        var loginResponse = await _httpClient.PostAsync("", loginRequest);
        Assert.NotNull(loginResponse);
        Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);

        var loginResponseContent = await loginResponse.Content.ReadAsStringAsync();
        var loginResponseObject = JsonConvert.DeserializeObject<SingleResponse<LoginResponse>>(loginResponseContent);
        Assert.NotNull(loginResponseObject);
        Assert.NotNull(loginResponseObject.Item);
        Assert.NotNull(loginResponseObject.Item.User);
        Assert.IsType<LoginResponse>(loginResponseObject.Item)
            ;
        Assert.NotNull(loginResponseObject.Item.AccessToken);
        var jwt = loginResponseObject.Item.AccessToken;
        var token = _factory.ValidateToken(jwt) as JwtSecurityToken;
        Assert.NotNull(token);

        Assert.IsType<UserResponse>(loginResponseObject.Item.User);
        Assert.Equal(_testUser, loginResponseObject.Item.User.Username);
    }
    
    /// <summary>
    /// Tests happy path with forgot password
    /// </summary>
    [Fact, Trait("Category", "Integration")]
    public async Task ForgotPassword_ValidUser_ShouldSucceed()
    {
        var forgotPasswordRequest = new { username = _testUser };
        var postBody = new StringContent(JsonSerializer.Serialize(forgotPasswordRequest), Encoding.UTF8, "application/json");
        var forgotPasswordResponse = await _httpClient.PostAsync("forgotpassword", postBody);
        Assert.NotNull(forgotPasswordResponse);
        Assert.Equal(HttpStatusCode.OK, forgotPasswordResponse.StatusCode);
        
        var content = await forgotPasswordResponse.Content.ReadAsStringAsync();
        var itemResponse = JsonConvert.DeserializeObject<SingleResponse<ForgotPasswordResponse>>(content);
        Assert.NotNull(itemResponse);
        Assert.IsType<SingleResponse<ForgotPasswordResponse>>(itemResponse);
    }
}