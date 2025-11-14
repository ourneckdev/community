using community.common.BaseClasses;
using community.common.Definitions;
using community.common.Enumerations;
using community.common.Exceptions;
using community.data.entities;
using community.data.entities.Search;
using community.data.postgres.Contexts;
using community.data.postgres.Interfaces;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace community.data.postgres.Repositories;

/// <summary>
/// </summary>
/// <param name="context"></param>
/// <param name="contextAccessor"></param>
/// <param name="logger"></param>
public class CommunityRepository(
    ICommonDapperContext context,
    IHttpContextAccessor contextAccessor,
    ILogger<CommunityRepository> logger)
    : BaseRepository(contextAccessor), ICommunityRepository
{
    /// <inheritdoc />
    public async Task<Guid> AddAsync(Community entity, CancellationToken cancellationToken = default)
    {
        try
        {
            await using var connection = context.CreateConnection();
            var communityId = await connection.ExecuteScalarAsync<Guid>(
                new CommandDefinition(
                    """
                    insert into community (name, description, website, number_of_parcels, parcel_size, 
                                           number_of_residents, average_home_value, created_by, modified_by)
                    values (
                           @Name
                         , @Description
                         , @Website
                         , @NumberOfParcels
                         , @ParcelSize
                         , @NumberOfResidents
                         , @AverageHomeValue
                         , @ModifiedBy
                         , @ModifiedBy)
                    returning id;
                    """, entity, cancellationToken: cancellationToken));

            return communityId;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ErrorCodes.DatabaseError_AddCommunity);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<bool> AddUserToCommunityAsync(Guid userId, Guid communityId, CancellationToken cancellationToken = default)
    {
        try
        {
            await using var connection = context.CreateConnection();
            var recordsAffected = await connection.ExecuteAsync(
                new CommandDefinition(
                    """
                    if not exists(select 1 from user_community where user_id = @userId and community_id = @communityId)
                        insert into user_community (user_id, community_id)
                        values (@userId, @communityId);
                    """, new { userId, communityId }, cancellationToken: cancellationToken));

            return recordsAffected == 1;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ErrorCodes.DatabaseError_AddUserToCommunity);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<Community> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await using var connection = context.CreateConnection();
        var reader = await connection.QueryMultipleAsync(
            new CommandDefinition(
                """
                select *
                  from community
                 where id = @id;

                select a.*
                  from community_address a
                 where a.community_id = @id
                   and a.is_active;

                select c.*
                  from contact c
                 where community_id = @communityId
                   and user_id is null
                   and c.is_active;
                """, new { id, communityId = CurrentCommunityId }, cancellationToken: cancellationToken));

        var community = (await reader.ReadAsync<Community>()).FirstOrDefault();

        if (community == null) throw new NotFoundException(ErrorCodes.Community_CommunityNotFound);

        community.Addresses = await reader.ReadAsync<CommunityAddress>();
        community.ContactMethods = await reader.ReadAsync<Contact>();

        logger.LogInformation($"User found with id {id}.");

        return community;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Community>> ListAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default)
    {
        await using var connection = context.CreateConnection();
        var reader = await connection.QueryMultipleAsync(
            new CommandDefinition(
                """
                select *
                  from community
                 where id = any(@ids);

                select a.*
                  from community_address a
                 where a.community_id = any(@ids)
                   and a.is_active;

                select c.*
                  from contact c
                 where community_id = any(@ids)
                   and entity_type = @entity_type
                   and c.is_active;
                """, new { ids, entity_type = EntityType.Community }, cancellationToken: cancellationToken));

        var communities = (await reader.ReadAsync<Community>()).ToList();
        if (communities == null) throw new NotFoundException(ErrorCodes.Community_CommunityNotFound);

        var addresses = (await reader.ReadAsync<CommunityAddress>()).ToList();
        var contacts = (await reader.ReadAsync<Contact>()).ToList();

        var communityAddresses = communities
            .GroupJoin(addresses,
                c => c.Id,
                a => a.CommunityId,
                (c, a) => new Community(c, a)).ToList();

        var communityContacts = communityAddresses
            .GroupJoin(contacts,
                c => c.Id,
                m => m.CommunityId,
                (c, m) => new Community(c, m)).ToList();

        logger.LogInformation("Communities found using ids {ids}", ids);
        return communityContacts;
    }

    /// <inheritdoc />
    public async Task<IList<CommunitySearchResults>> FindCommunityAsync(FindCommunityRecord search, CancellationToken cancellationToken = default)
    {
        await using var connection = context.CreateConnection();
        var matches = (await connection.QueryAsync<CommunitySearchResults>(
            new CommandDefinition(search.BuildQuery(), search, cancellationToken: cancellationToken))).ToList();

        if (!matches.Any()) throw new NotFoundException(ErrorCodes.Community_CommunityNotFound);
        return matches;
    }
}