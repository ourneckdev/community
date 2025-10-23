using community.common.Interfaces;
using community.models.Requests.Users;
using community.models.Responses;
using community.models.Responses.Base;

namespace community.providers.community.Interfaces;

/// <summary>
///     Encapsulates all the business logic that pertains to a user.
/// </summary>
public interface IUserProvider : IProvider
{
    /// <summary>
    ///     Retrieves full profile for user
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<SingleResponse<UserResponse>> GetAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Validates a username as valid phone or email address.
    ///     Sends a confirmation to the user.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<SingleResponse<bool>> VerifyUserNameAsync(VerifyUserNameRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Adds a user
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<SingleResponse<Guid>> AddAsync(AddUserRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Updates an existing user.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>A response object encapsulating a true/false response indicating success of the operation.</returns>
    Task<SingleResponse<bool>> UpdateAsync(UpdateUserRequest request, CancellationToken cancellationToken = default);
}