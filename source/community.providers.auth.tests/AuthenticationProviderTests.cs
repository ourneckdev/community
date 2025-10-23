using System.IdentityModel.Tokens.Jwt;
using community.common.Definitions;
using community.common.Exceptions;
using community.common.Utilities;
using community.data.entities;
using community.data.postgres.Interfaces;
using community.models.Requests.Authentication;
using community.models.Responses.Authentication;
using community.models.Responses.Base;
using community.providers.auth.Implementation;
using community.providers.auth.Interfaces;
using community.tests.common;
using community.tests.common.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace community.providers.auth.tests;

/// <summary>
///     Unit tests for auth provider.
/// </summary>
public class AuthenticationProviderTests : BaseTest
{
    private static readonly string UserName = nameof(UserName);
    private readonly Mock<IAuthenticationRepository> _mockAuthenticationRepository = new();
    private readonly Mock<ILogger<AuthenticationProvider>> _mockLogger = new();
    private readonly Mock<IUserRepository> _mockUserRepository = new();
    private readonly IAuthenticationProvider _provider;
    private readonly ITokenProvider _tokenProvider;
    private readonly Guid _userId;

    /// <summary>
    ///     Initialize the dependencies for the test
    /// </summary>
    public AuthenticationProviderTests()
    {
        _tokenProvider = new TokenProvider(MockOptions.Object, MockHttpContextAccessor.Object,
            NullLogger<TokenProvider>.Instance);

        _provider = new AuthenticationProvider(_mockAuthenticationRepository.Object, _mockUserRepository.Object,
            _tokenProvider, MockHttpContextAccessor.Object, _mockLogger.Object);

        _userId = Guid.NewGuid();
    }

    /// <summary>
    ///     Test the happy path for generating a login code.
    /// </summary>
    [Fact]
    [Trait("TestCategory", "Unit")]
    public async Task RequestLoginCode_ValidRequest_ShouldSucceed()
    {
        _mockUserRepository
            .Setup(u => u.UserExistsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_userId);

        _mockAuthenticationRepository
            .Setup(a => a.SetLoginCode(It.IsAny<Guid>(), It.IsAny<string>()));

        var loginCode = await _provider.RequestLoginCodeAsync(UserName);

        Assert.NotNull(loginCode);
        Assert.IsType<SingleResponse<LoginCodeResponse>>(loginCode);
        Assert.IsType<string>(loginCode.Item.Code);
        Assert.True(loginCode.Item.Code.Length == 6);

        _mockUserRepository.Verify(u => u.UserExistsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Once);
        _mockAuthenticationRepository.Verify(a => a.SetLoginCode(It.IsAny<Guid>(), It.IsAny<string>()), Times.Once);
    }

    /// <summary>
    ///     Tests that a <see cref="NotFoundException" /> is thrown if the user doesn't exist.
    /// </summary>
    [Fact]
    [Trait("TestCategory", "Unit")]
    public async Task RequestLoginCode_UserDoesNotExist_ShouldSucceed()
    {
        _mockUserRepository
            .Setup(u => u.UserExistsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new NotFoundException(ErrorCodes.User_UserNotFound));

        _mockAuthenticationRepository
            .Setup(a => a.SetLoginCode(It.IsAny<Guid>(), It.IsAny<string>()));

        await Assert.ThrowsAsync<NotFoundException>(async () => await _provider.RequestLoginCodeAsync(UserName));

        _mockUserRepository.Verify(u => u.UserExistsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Once);
        _mockAuthenticationRepository.Verify(a => a.SetLoginCode(It.IsAny<Guid>(), It.IsAny<string>()), Times.Never);
    }

    /// <summary>
    ///     Login with code, happy path
    /// </summary>
    [Fact]
    [Trait("TestCategory", "Unit")]
    public async Task LoginAsync_ValidRequest_ShouldSucceed()
    {
        var request =
            new LoginWithCodeRequest(nameof(LoginWithCodeRequest.Username),
                nameof(LoginWithCodeRequest.LoginCode));

        _mockAuthenticationRepository
            .Setup(a => a.LoginAsync(UserName, It.IsAny<string>()))
            .ReturnsAsync(Guid.NewGuid());

        _mockUserRepository
            .Setup(u => u.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Entities.ValidUser);

        var response = await _provider.LoginAsync(request);
        Assert.NotNull(response);
        Assert.IsType<SingleResponse<LoginResponse>>(response);
        Assert.NotNull(response.Item);
        Assert.IsType<JwtSecurityToken>(_tokenProvider.ValidateToken(response.Item.AccessToken));

        _mockAuthenticationRepository.Verify(a => a.LoginAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        _mockUserRepository.Verify(u => u.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    /// <summary>
    ///     Tests the validation of a null login code
    /// </summary>
    [Fact]
    [Trait("TestCategory", "Unit")]
    public async Task LoginAsync_MissingLoginCode_ThrowsValidationError_ShouldSucceed()
    {
        var request = new LoginWithCodeRequest(nameof(LoginWithPasswordRequest.Username), null);
        var response = await Assert.ThrowsAsync<ValidationException>(async () => await _provider.LoginAsync(request));
        Assert.Equal(ValidationMessages.ValidationErrors, response.Message);
        Assert.True(response.Errors.Keys.Contains(nameof(LoginWithCodeRequest.LoginCode)));
        Assert.True(response.Errors[nameof(LoginWithCodeRequest.LoginCode)]
            .SequenceEqual([ValidationMessages.LoginCodeRequired]));
    }

    /// <summary>
    ///     Tests logging in with a password, happy path
    /// </summary>
    [Fact]
    [Trait("TestCategory", "Unit")]
    public async Task LoginWithPasswordAsync_ValidUser_ShouldSucceed()
    {
        var request = new LoginWithPasswordRequest(
            nameof(LoginWithPasswordRequest.Username),
            nameof(LoginWithPasswordRequest.Password)
        );
        
        _mockUserRepository
            .Setup(u => u.UserExistsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_userId);
        
        _mockUserRepository
            .Setup(u => u.GetAsync(_userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Entities.ValidUser);

        var loginResponse = await _provider.LoginWithPasswordAsync(request);
        Assert.NotNull(loginResponse);
        Assert.IsType<SingleResponse<LoginResponse>>(loginResponse);
        Assert.Equal(loginResponse.Item.User.Id, Entities.ValidUser.Id);
        Assert.IsType<JwtSecurityToken>(_tokenProvider.ValidateToken(loginResponse.Item.AccessToken));
        
        _mockUserRepository.Verify(u => u.UserExistsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        _mockUserRepository.Verify(u => u.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    /// <summary>
    ///     Tests logging in with an invalid password, throws validation exception
    /// </summary>
    [Fact]
    [Trait("TestCategory", "Unit")]
    public async Task LoginWithPasswordAsync_ValidUserInvalidPassword_ThrowsValidationException_ShouldSucceed()
    {
        var request = new LoginWithPasswordRequest(
            nameof(LoginWithPasswordRequest.Username),
            "IncorrectPassword"
        );
        
        _mockUserRepository
            .Setup(u => u.UserExistsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_userId);
        
        _mockUserRepository
            .Setup(u => u.GetAsync(_userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Entities.ValidUser);

        var response = await Assert.ThrowsAsync<ValidationException>(async() =>  await _provider.LoginWithPasswordAsync(request));
        Assert.NotNull(response);
        Assert.IsType<ValidationException>(response);
        Assert.Equal(ErrorCodes.Login_IncorrectPassword, response.Message);
        
        _mockUserRepository.Verify(u => u.UserExistsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        _mockUserRepository.Verify(u => u.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}