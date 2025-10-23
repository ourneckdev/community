using community.common.BaseClasses;
using community.common.Definitions;
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
            using var connection = context.CreateConnection();
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
    public async Task<bool> AddUserToCommunityAsync(Guid userId, Guid communityId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            using var connection = context.CreateConnection();
            var recordsAffected = await connection.ExecuteAsync(
                """
                if not exists(select 1 from user_community where user_id = @userId and community_id = @communityId)
                    insert into user_community (user_id, community_id)
                    values (@userId, @communityId);
                """, new { userId, communityId });

            return recordsAffected == 1;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ErrorCodes.DatabaseError_AddUserToCommunity);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<IList<CommunitySearchResults>> FindCommunityAsync(FindCommunityRecord search,
        CancellationToken cancellationToken = default)
    {
        try
        {
            using var connection = context.CreateConnection();

            var matches = await connection.QueryAsync<CommunitySearchResults>(
                new CommandDefinition(
                    """
                    select c.id, c.name, cast(null as uuid) address_id, cast(null as uuid) contact_id
                      from community c
                     where levenshtein(c.name, @Name, 1, 0, 4) <= 3
                     union
                    select c.id, c.name, a.id address_id, cast(null as uuid) contact_id
                      from community_address a
                      join community c on a.community_id = c.id
                     where (@StateCode is null or upper(a.state_code) = @StateCode)
                       and (@City is null or lower(a.city) = lower(@City))
                       and (@PostalCode is null or a.postal_code = @PostalCode)
                       and (@AddressLine1 is null or levenshtein(lower(address_1), lower(@AddressLine1), 5, 0, 4) <= 4)
                     union
                    select c.id, c.name, cast(null as uuid) address_id, t.id contact_id
                      from contact t 
                      join contact_method m on t.contact_method_id = m.id
                       and m.contact_type = 'phone'
                      join community c on c.id = t.community_id
                       and entity_type = 0
                       and (@PhoneNumber is null or t.value = @PhoneNumber)
                    """, search, cancellationToken: cancellationToken));

            return matches.ToList();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ErrorCodes.DatabaseError_SearchFailed);
            return [];
        }
    }
}