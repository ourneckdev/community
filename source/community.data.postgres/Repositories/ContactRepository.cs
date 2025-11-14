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
/// </summary>
/// <param name="commonDapperContext"></param>
/// <param name="contextAccessor">context access for accessing properties from the http pipeline</param>
/// <param name="logger"></param>
public class ContactRepository(
    ICommonDapperContext commonDapperContext,
    IHttpContextAccessor contextAccessor,
    ILogger<ContactRepository> logger)
    : BaseRepository(contextAccessor), IContactRepository
{
    /// <inheritdoc />
    public async Task<Guid> AddAsync(Contact contact, CancellationToken cancellationToken = default)
    {
        try
        {
            await using var connection = commonDapperContext.CreateConnection();
            var id = await connection.ExecuteScalarAsync<Guid>(
                new CommandDefinition(
                    """
                    insert into contact(community_id, user_id, entity_type, contact_method_id, value, visible, can_contact, created_by, modified_by) 
                    values (
                           @CommunityId
                         , @UserId
                         , @EntityType
                         , @ContactMethodId
                         , @Value
                         , @Visible
                         , @CanContact
                         , @ModifiedBy
                         , @ModifiedBy)
                    returning id;
                    """, contact, cancellationToken: cancellationToken));

            await LogContactConsent(id, contact.CommunityId, contact.CanContact, cancellationToken);

            return id;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ErrorCodes.DatabaseError_AddContact);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task UpdateAsync(Contact contact, CancellationToken cancellationToken = default)
    {
        try
        {
            await using var connection = commonDapperContext.CreateConnection();
            await connection.ExecuteAsync(
                new CommandDefinition(
                    """
                    update contact
                       set value = @Value
                         , contact_method_id = @ContactMethodId
                         , visible = @Visible
                         , can_contact = @CanContact
                         , modified_by = @ModifiedBy
                         , modified_by = utcnow()
                     where id = @Id
                    """, contact, cancellationToken: cancellationToken));

            await LogContactConsent(contact.Id, contact.CommunityId, contact.CanContact, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ErrorCodes.DatabaseError_UpdateContact);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<Contact> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            await using var connection = commonDapperContext.CreateConnection();
            var contact = await connection.QueryFirstOrDefaultAsync(
                new CommandDefinition(
                    """
                    select *
                      from contact
                     where id = @id
                       and community_id = @CommunityId
                    """, new { id, CommunityId = CurrentCommunityId }, cancellationToken: cancellationToken));

            if (contact == null) throw new NotFoundException(ErrorCodes.Contact_NotFound);
            return contact;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ErrorCodes.DatabaseError_GetContact);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Contact>> ListAsync(Guid communityId, CancellationToken cancellationToken = default)
    {
        try
        {
            await using var connection = commonDapperContext.CreateConnection();
            return await connection.QueryAsync<Contact>(
                new CommandDefinition(
                    """
                    select *
                      from contact
                     where community_id = @communityId
                       and is_active
                       and entity_type = 'community'
                    """, communityId, cancellationToken: cancellationToken));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ErrorCodes.DatabaseError_ListCommunityContacts);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Contact>> ListAsync(Guid communityId, Guid userId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await using var connection = commonDapperContext.CreateConnection();
            return await connection.QueryAsync<Contact>(
                new CommandDefinition(
                    """
                    select *
                      from contact
                     where community_id = @communityId
                       and user_id = @userId
                       and is_active
                       and entity_type = 'user'
                    """, new { communityId, userId }, cancellationToken: cancellationToken));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ErrorCodes.DatabaseError_ListUserContacts);
            throw;
        }
    }


    private async Task LogContactConsent(Guid id, Guid communityId, bool canContact,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await using var connection = commonDapperContext.CreateConnection();
            await connection.ExecuteAsync(
                new CommandDefinition(
                    """
                    insert into contact_consent_log (contact_id, community_id, has_consent)
                    values (@id, @communityId, @canContact)
                    """, new { id, communityId, canContact }, cancellationToken: cancellationToken));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ErrorCodes.DatabaseError_AddContactConsentLog);
            throw;
        }
    }
}