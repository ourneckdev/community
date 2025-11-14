using community.common.Definitions;
using community.common.Exceptions;
using community.data.postgres.Contexts;
using community.data.postgres.Interfaces;
using Dapper;

namespace community.data.postgres.Repositories;

/// <summary>
///     Encapsulates repository methods required for interacting with the database on login.
/// </summary>
/// <param name="context">The Dapper context for managing pooled connections.</param>
public class AuthenticationRepository(IAuthorizationDapperContext context)
    : IAuthenticationRepository
{
    private static readonly TimeSpan LoginInterval = TimeSpan.FromMinutes(15);

    /// <inheritdoc cref="IAuthenticationRepository.LoginAsync" />
    public async Task<Guid> LoginAsync(string username, string passwordCode)
    {
        await using var connection = context.CreateConnection();
        var userId = await connection.QuerySingleOrDefaultAsync<Guid?>(
            """
            select id
              from "user"
             where username = @username
               and login_code = @passwordCode
               and extract(epoch from login_code_expiration - utcnow()) >= 0
               and is_active
               and not locked
               and username_verified;
            """, new { username, passwordCode });

        if (userId == null)
            throw new BusinessRuleException(ErrorCodes.Login_LoginCodeExpired);

        await SetLastLoginDate(userId.Value);

        return userId.Value;
    }

    /// <inheritdoc cref="IAuthenticationRepository.SetUserLastCommunityAsync" />
    public async Task SetUserLastCommunityAsync(Guid userId, Guid communityId)
    {
        await using var connection = context.CreateConnection();

        await connection.ExecuteAsync(
            """
            update "user"
               set last_community_id = @communityId
                 , modified_date = utcnow()
                 , modified_by = @userId
             where id = @userId
            """, new { userId, communityId });
    }

    /// <inheritdoc cref="IAuthenticationRepository.SetLoginCode" />
    public async Task SetLoginCode(Guid userId, string code)
    {
        var connection = context.CreateConnection();
        await connection.ExecuteAsync(
            """
            update "user"
               set login_code = @code
                 , login_code_expiration = utcnow() + @interval
             where id = @userId
            """, new { userId, code, interval = LoginInterval });
    }

    /// <inheritdoc cref="IAuthenticationRepository.SetLastLoginDate" />
    public async Task SetLastLoginDate(Guid userId)
    {
        await using var connection = context.CreateConnection();
        await connection.ExecuteAsync(
            """
            update "user" 
               set last_login_date = utcnow() 
                 , login_code = null
                 , login_code_expiration = null
             where id = @userId;
            """, new { userId });
    }
}