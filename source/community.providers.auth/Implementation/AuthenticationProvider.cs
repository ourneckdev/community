using System.Text;
using community.common.BaseClasses;
using community.common.Definitions;
using community.common.Exceptions;
using community.common.Extensions;
using community.common.Utilities;
using community.data.postgres.Interfaces;
using community.models.Requests.Authentication;
using community.models.Responses.Authentication;
using community.models.Responses.Base;
using community.providers.auth.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace community.providers.auth.Implementation;

/// <summary>
///     Implements the available methods for enabling a user to log in.
/// </summary>
/// <param name="authRepository">The authentication repository.</param>
/// <param name="userRepository">the user repository</param>
/// <param name="tokenProvider">the token provider responsible for generating JWTs</param>
/// <param name="contextAccessor">the context accessor for reading data from the http pipeline.</param>
/// <param name="logger">the class level logger.</param>
public class AuthenticationProvider(
    IAuthenticationRepository authRepository,
    IUserRepository userRepository,
    ITokenProvider tokenProvider,
    IHttpContextAccessor contextAccessor,
    ILogger<AuthenticationProvider> logger)
    : BaseProvider(contextAccessor), IAuthenticationProvider
{
    private readonly Dictionary<string, string[]> _validationErrors = new();

    /// <inheritdoc cref="IAuthenticationProvider.RequestLoginCodeAsync" />
    public async ValueTask<SingleResponse<LoginCodeResponse>> RequestLoginCodeAsync(string username)
    {
        var response = await MeasureExecutionAsync(async () =>
        {
            try
            {
                var userId = await userRepository.UserExistsAsync(username);
                var loginCode = GenerateLoginCode();
                await authRepository.SetLoginCode(userId, loginCode);
                return new SingleResponse<LoginCodeResponse>(new LoginCodeResponse());
            }
            catch (NotFoundException ex)
            {
                logger.LogWarning(ex, ex.Message);
                throw;
            }
        });

        return response;
    }

    /// <inheritdoc cref="IAuthenticationProvider.LoginAsync" />
    public async Task<SingleResponse<LoginResponse>> LoginAsync(LoginWithCodeRequest withCodeRequest)
    {
        var response = await MeasureExecutionAsync(async () =>
        {
            _validationErrors.Clear();

            if (string.IsNullOrEmpty(withCodeRequest.Username))
                _validationErrors.Add(nameof(withCodeRequest.Username), [ValidationMessages.UsernameRequired]);

            if (string.IsNullOrEmpty(withCodeRequest.LoginCode))
                _validationErrors.Add(nameof(withCodeRequest.LoginCode), [ValidationMessages.LoginCodeRequired]);

            if (_validationErrors.Any())
                throw new ValidationException(ValidationMessages.ValidationErrors, _validationErrors);

            var userId = await authRepository.LoginAsync(withCodeRequest.Username, withCodeRequest.LoginCode!);
            var user = await userRepository.GetAsync(userId);
            var loginResponse = tokenProvider.GenerateTokens(user);

            logger.LogInformation("Login successful: {{ \n" +
                                  @$"\t""user id"": ""{userId}"",\n" +
                                  @$"\t""access token"": ""{loginResponse.AccessToken.MaskString()}"",\n" +
                                  @$"\t""refresh token"": ""{loginResponse.RefreshToken.MaskString(10)}"",\n" +
                                  @$"\t""log time"": ""{DateTime.UtcNow}"",\n" +
                                  "}}");

            return new SingleResponse<LoginResponse>(loginResponse);
        });

        return response;
    }

    /// <inheritdoc cref="IAuthenticationProvider.LoginWithPasswordAsync" />
    public async Task<SingleResponse<LoginResponse>> LoginWithPasswordAsync(LoginWithPasswordRequest withCodeRequest)
    {
        var response = await MeasureExecutionAsync(async () =>
        {
            _validationErrors.Clear();

            if (string.IsNullOrEmpty(withCodeRequest.Username))
                _validationErrors.Add(nameof(withCodeRequest.Username), [ValidationMessages.UsernameRequired]);

            if (string.IsNullOrEmpty(withCodeRequest.Password))
                _validationErrors.Add(nameof(withCodeRequest.Password), [ValidationMessages.PasswordRequired]);

            try
            {
                var userId = await userRepository.UserExistsAsync(withCodeRequest.Username);
                var user = await userRepository.GetAsync(userId);

                if (string.IsNullOrEmpty(user.Password))
                    throw new ValidationException(ErrorCodes.Login_IncorrectPassword
                        , new Dictionary<string, string[]> { { nameof(user.Password), [ValidationMessages.PasswordRequired] } });

                var decryptedPassword = EncryptionHelper.Decrypt(user.Password);

                if (withCodeRequest.Password != decryptedPassword)
                    throw new ValidationException(ErrorCodes.Login_IncorrectPassword
                        , new Dictionary<string, string[]> { { nameof(user.Password), [ValidationMessages.PasswordRequired] } });

                var accessToken = tokenProvider.GenerateAccessToken(user);
                var refreshToken = tokenProvider.GenerateRefreshToken(user.Id);

                logger.LogInformation("Login with password successful: {{ \n" +
                                      @$"\t""user id"": ""{user.Id}"",\n" +
                                      @$"\t""access token"": ""{accessToken.MaskString()}"",\n" +
                                      @$"\t""refresh token"": ""{refreshToken.MaskString(10)}"",\n" +
                                      @$"\t""log time"": ""{DateTime.UtcNow}"",\n" +
                                      "}}");

                return new SingleResponse<LoginResponse>(new LoginResponse(accessToken, refreshToken, user));
            }
            catch (BusinessRuleException ex)
            {
                logger.LogError(ex, ex.Message);
                throw;
            }
        });
        return response;
    }

    /// <inheritdoc cref="IAuthenticationProvider.ForgotPasswordAsync" />
    public async Task<SingleResponse<ForgotPasswordResponse>> ForgotPasswordAsync(
        ForgotPasswordRequest forgotPasswordRequest)
    {
        var response = await MeasureExecutionAsync(async () =>
        {
            var userId = await userRepository.UserExistsAsync(forgotPasswordRequest.Username);
            var loginCode = GenerateLoginCode();
            await authRepository.SetLoginCode(userId, loginCode);
            //todo: send login code via email or sms
            return new SingleResponse<ForgotPasswordResponse>(new ForgotPasswordResponse());
        });

        return response;
    }

    #region Private Methods

    /// <summary>
    ///     Generates a {Integers.LoginCodeLength} length string of numbers to use as a login code.
    /// </summary>
    /// <returns>A 6 digit string</returns>
    private string GenerateLoginCode()
    {
        var random = new Random();
        var loginCode = new StringBuilder(Integers.LoginCodeLength);

        for (var i = 0; i < Integers.LoginCodeLength; i++)
            loginCode.Append(random.Next(0, 9));

        return loginCode.ToString();
    }

    #endregion
}