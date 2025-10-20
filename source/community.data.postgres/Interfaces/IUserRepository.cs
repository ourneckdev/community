using community.common.Exceptions;
using community.common.Interfaces;
using community.data.entities;

namespace community.data.postgres.Interfaces;

/// <summary>
///     Defines the available repository methods to query user data.
/// </summary>
public interface IUserRepository : IRepository
{
    /// <summary>
    ///     Queries the database to discover if a username already exists.
    /// </summary>
    /// <param name="username">The username to validate for existence.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>A true/false indicator as to whether the username already exists.</returns>
    Task<bool> UsernameExistsAsync(string username, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Queries the database to validate a username (phone or email) was found and logging in can proceed.
    ///     The query matches the username and requires and active, verified and unlocked account.
    /// </summary>
    /// <param name="username">The phone number or email address the user signed up with.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>The ID of the found user record.</returns>
    Task<Guid> UserExistsAsync(string username, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Retrieves a single user by id
    /// </summary>
    /// <param name="id">the database id to retrieve the record against.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>A user and it's accompanying information</returns>
    /// <exception cref="NotFoundException">Exception thrown when the user does not exist in the database.</exception>
    Task<User> GetAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Adds a user record to the database.
    /// </summary>
    /// <param name="user">The user object to save.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>The generated ID of the user</returns>
    Task<Guid> AddAsync(User user, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Updates all editable aspects of a user record
    /// </summary>
    /// <param name="user">The user object to persist changes against.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>A boolean indicator of success.</returns>
    Task<bool> UpdateAsync(User user, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Marks a user record as verified, recording the date the record was marked as well as the user who did the
    ///     verification.
    /// </summary>
    /// <param name="id">The ID of the user being verified.</param>
    /// <param name="communityId">The ID of the community the user is being verified against</param>
    /// <param name="verifiedBy">The ID of the Company Administrator who is doing the verification.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>A boolean indicating success.</returns>
    Task<bool> MarkUserVerified(Guid id, Guid communityId, Guid verifiedBy, CancellationToken cancellationToken = default);

    /// <summary>
    /// Marks a username as verified by supplying the login code sent to the user's phone or email.
    /// </summary>
    /// <param name="username">the username to verify</param>
    /// <param name="code">the generated login code</param>
    /// <param name="cancellationToken"></param>
    /// <returns>true/false based on operation</returns>
    Task<bool> MarkUsernameVerified(string username, string code, CancellationToken cancellationToken = default);
    
    
}