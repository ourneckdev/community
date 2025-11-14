using community.common.BaseClasses;
using community.common.Definitions;
using community.common.Exceptions;
using community.data.entities;
using community.data.postgres.Contexts;
using community.data.postgres.Interfaces;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace community.data.postgres.Repositories;

/// <summary>
///     Collection of repository methods for interacting with a user.
/// </summary>
/// <param name="context">The dapper context initialized within the container.</param>
/// <param name="logger">The logger for the <see cref="UserRepository" /></param>
/// <param name="contextAccessor">context access for accessing properties from the http pipeline</param>
public class UserRepository(
    IAuthorizationDapperContext context,
    ILogger<UserRepository> logger,
    IHttpContextAccessor contextAccessor)
    : BaseRepository(contextAccessor), IUserRepository
{
    /// <inheritdoc />
    public async Task<bool> UsernameExistsAsync(string username, CancellationToken cancellationToken = default)
    {
        await using var connection = context.CreateConnection();
        var exists = await connection.QuerySingleAsync<bool>(
            new CommandDefinition(
                """
                select exists(
                select id
                  from "user"
                 where username = @username
                     )
                """, new { username }, cancellationToken: cancellationToken));

        return exists;
    }

    /// <inheritdoc />
    public async Task<Guid> UserExistsAsync(string username, CancellationToken cancellationToken = default)
    {
        await using var connection = context.CreateConnection();
        var userId = await connection.ExecuteScalarAsync<Guid?>(
            new CommandDefinition(
                """
                select id
                  from "user" 
                 where username=@username
                   and is_active
                   and not locked
                   and username_verified
                """, new { username }, cancellationToken: cancellationToken));

        if (userId == null) throw new NotFoundException(ErrorCodes.User_UserNotFound);

        return userId.Value;
    }

    /// <inheritdoc />
    public async Task<User> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await using var connection = context.CreateConnection();

        var reader = await connection.QueryMultipleAsync(
            new CommandDefinition(
                """
                select *
                  from "user" u
                 where id = @id;

                select a.*
                  from user_address a
                 where a.user_id = @id
                   and a.is_active;

                select c.*
                  from contact c
                 where user_id = @id
                   and community_id = @communityId
                   and c.is_active;

                select c.*
                  from community c
                  join user_community uc on c.id = uc.community_id
                 where uc.user_id = @id
                   and c.is_active
                   and uc.is_active;
                """, new { id, communityId = CurrentCommunityId }, cancellationToken: cancellationToken));

        var user = (await reader.ReadAsync<User>()).FirstOrDefault();

        if (user == null) throw new NotFoundException(ErrorCodes.User_UserNotFound);

        user.Addresses = await reader.ReadAsync<UserAddress>();
        user.ContactMethods = await reader.ReadAsync<Contact>();
        user.Communities = await reader.ReadAsync<Community>();

        logger.LogInformation($"User found with id {id}.");

        return user;
    }

    /// <inheritdoc />
    public async Task<Guid> AddAsync(User user, CancellationToken cancellationToken = default)
    {
        await using var connection = context.CreateConnection();
        var userId = await connection.ExecuteScalarAsync<Guid>(
            new CommandDefinition(
                """
                insert into "user" (
                       user_type_id
                     , username
                     , password
                     , prefix
                     , firstname
                     , lastname
                     , suffix
                     , date_of_birth
                     , last_community_id
                     , created_by
                     , modified_by
                     )
                values (
                       @UserTypeId
                     , @Username
                     , @Password
                     , @Prefix
                     , @FirstName
                     , @LastName
                     , @Suffix
                     , @DateOfBirth
                     , @LastCommunityId
                     , @ModifiedBy
                     , @ModifiedBy
                     )
                returning Id;
                """, user, cancellationToken: cancellationToken));

        return userId;
    }

    /// <inheritdoc />
    public async Task<bool> UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        await using var connection = context.CreateConnection();
        var recordsAffected = await connection.ExecuteAsync(
            new CommandDefinition(
                """
                update "user"
                   set user_type_id = @UserTypeId
                     , firstname = @FirstName
                     , lastname = @Lastname  
                     , prefix = @Prefix
                     , suffix = @Suffix
                     , locked = @Locked
                     , profile_pic = @ProfilePic
                     , date_of_birth = @DateOfBirth
                     , modified_date = utcnow()
                     , modified_by = @ModifiedBy
                 where id = @user.Id;
                """, user, cancellationToken: cancellationToken));

        return recordsAffected == 1;
    }

    /// <inheritdoc />
    public async Task<bool> MarkUsernameVerified(string username, string code,
        CancellationToken cancellationToken = default)
    {
        await using var connection = context.CreateConnection();
        var exists = await connection.ExecuteScalarAsync<bool>(
            new CommandDefinition(
                """
                select exists(
                select id
                  from "user"
                 where username = @username
                   and login_code = @code
                   and extract(epoch from login_code_expiration - utcnow()) >= 0
                     )
                """, new { username, code }, cancellationToken: cancellationToken));

        if (!exists) throw new NotFoundException(ErrorCodes.User_UserNotFound);

        var recordsAffected = await connection.ExecuteAsync(
            new CommandDefinition(
                """
                update "user"
                  set username_verified = true
                    , username_verified_date = utcnow()
                    , locked = false
                where username = @username
                """, new { username }, cancellationToken: cancellationToken));

        return recordsAffected == 1;
    }

    #region UserCommunity DB Calls

    /// <inheritdoc />
    public async Task<bool> MarkUserVerified(Guid id, Guid communityId, Guid verifiedBy,
        CancellationToken cancellationToken = default)
    {
        await using var connection = context.CreateConnection();
        var recordsAffected = await connection.ExecuteAsync(
            new CommandDefinition(
                """
                update user_community
                   set verified = true
                     , verified_date = utcnow()
                     , verified_by = @verifiedBy
                 where user_id = @id
                   and community_id = @communityId;
                """, new { id, communityId, verifiedBy }, cancellationToken: cancellationToken));

        return recordsAffected == 1;
    }

    #endregion
}