using community.common.Interfaces;

namespace community.data.postgres.Interfaces;

/// <summary>
///     Defines the available repository methods related to authentication.
/// </summary>
public interface IAuthenticationRepository : IRepository
{
    /// <summary>
    ///     Using the username (phone or email user signed up with), and the password code supplied to the user,
    ///     we query the database to ensure an active user record is found within the expiration period (6 minutes)
    /// </summary>
    /// <param name="username">the username of the user logging in.</param>
    /// <param name="passwordCode">the generated login code to check against what's stored in the database.</param>
    /// <returns></returns>
    Task<Guid> LoginAsync(string username, string passwordCode);

    /// <summary>
    ///     Updates the user's record with the current community they are accessing.
    ///     Allows for the user being placed back within this community when they login next.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="communityId"></param>
    /// <returns></returns>
    Task SetUserLastCommunityAsync(Guid userId, Guid communityId);

    /// <summary>
    ///     Sets the login code on the user record to begin the countdown to expiration of the code.
    /// </summary>
    /// <param name="userId">The ID of the user logging in.</param>
    /// <param name="code">The generated code to set in the database.</param>
    /// <returns></returns>
    Task SetLoginCode(Guid userId, string code);

    /// <summary>
    ///     Sets the last login date for a user.
    /// </summary>
    /// <param name="userId">The user to set last login date to current utc date.</param>
    Task SetLastLoginDate(Guid userId);
}