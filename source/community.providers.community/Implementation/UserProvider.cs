using community.common.BaseClasses;
using community.common.content;
using community.common.Definitions;
using community.common.Exceptions;
using community.common.Extensions;
using community.data.postgres.Interfaces;
using community.data.postgres.Repositories;
using community.models.BusinessObjects;
using community.models.Requests.Users;
using community.models.Responses;
using community.models.Responses.Base;
using community.providers.common.Interfaces;
using community.providers.community.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace community.providers.community.Implementation;

/// <summary>
///     Encapsulates methods required for manipulating user data.
/// </summary>
/// <param name="userRepository"></param>
/// <param name="notificationFactory"></param>
/// <param name="contextAccessor"></param>
/// <param name="logger"></param>
// /// <param name="contactMethodRepository"></param>
// /// <param name="addressRepository"></param>
// /// <param name="communityRepository"></param>
public class UserProvider(
    IUserRepository userRepository,
    // ICommunityRepository communityRepository,
    // IContactRepository contactMethodRepository,
    // IAddressRepository addressRepository,
    INotificationFactory notificationFactory,
    IHttpContextAccessor contextAccessor,
    ILogger<UserRepository> logger)
    : BaseProvider(contextAccessor), IUserProvider
{
    /// <inheritdoc />
    public async Task<SingleResponse<Guid>> AddAsync(AddUserRequest request,
        CancellationToken cancellationToken = default)
    {
        await Task.CompletedTask;
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public async Task<SingleResponse<bool>> UpdateAsync(UpdateUserRequest request,
        CancellationToken cancellationToken = default)
    {
        var response = await MeasureExecutionAsync(async () =>
        {
            request.Validate();
            var isSaved = await userRepository.UpdateAsync(request.ToUser(), cancellationToken);
            return new SingleResponse<bool>(isSaved);
        });
        logger.LogInformation(PrepareInformationLog(nameof(UpdateAsync), response.ExecutionMilliseconds));
        return response;
    }

    /// <inheritdoc />
    public async Task<SingleResponse<UserResponse>> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var response = await MeasureExecutionAsync(async () =>
        {
            var user = await userRepository.GetAsync(id, cancellationToken);
            return new SingleResponse<UserResponse>(user);
        });
        logger.LogInformation(PrepareInformationLog(nameof(GetAsync), response.ExecutionMilliseconds));
        return response;
    }

    /// <inheritdoc />
    public async Task<SingleResponse<bool>> VerifyUserNameAsync(VerifyUserNameRequest request,
        CancellationToken cancellationToken = default)
    {
        var response = await MeasureExecutionAsync(async () =>
        {
            var validationException = new ValidationException(ValidationMessages.ValidationErrors);
            if (string.IsNullOrEmpty(request.Code))
                validationException.AddError(nameof(request.Code), ValidationMessages.VerificationCodeNull);

            request.Validate(validationException);

            // if any validation errors have been recorded, throw the exception.
            if (validationException.Errors.Any())
                throw validationException;

            try
            {
                return new SingleResponse<bool>(
                    await userRepository.MarkUsernameVerified(request.Username, request.Code, cancellationToken));
            }
            catch
            {
                return new SingleResponse<bool>(false);
            }
        });

        logger.LogInformation(PrepareInformationLog(nameof(VerifyUserNameAsync), response.ExecutionMilliseconds));

        return response;
    }

    private async Task SendUsernameVerificationAsync(string username)
    {
        var notification = new Notification([username],
            "Verification code",
            ResourceNames.UsernameVerificationCode,
            username.IsValidEmailAddress());

        await notificationFactory
            .GetNotificationProvider(notification)
            .SendNotificationAsync(notification);
    }
}