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
///     Repository class necessary for extending
/// </summary>
/// <param name="context"></param>
/// <param name="contextAccessor">context access for accessing properties from the http pipeline</param>
/// <param name="logger">Logger interface for outputting unexpected errors.</param>
public class AddressRepository(
    ICommonDapperContext context,
    IHttpContextAccessor contextAccessor,
    ILogger<AddressRepository> logger)
    : BaseRepository(contextAccessor), IAddressRepository
{
    /// <inheritdoc />
    public async Task<Guid> AddUserAddressAsync(UserAddress userAddress, CancellationToken token = default)
    {
        try
        {
            using var connection = context.CreateConnection();
            var addressId = await connection.ExecuteScalarAsync<Guid>(
                new CommandDefinition(
                    """
                    insert into user_address (
                           community_id
                         , user_id
                         , address_type_id
                         , lot_number
                         , address_1
                         , address_2
                         , address_3
                         , city
                         , state_code
                         , postal_code
                         , county_code
                         , country_code
                         , timezone
                         , timezone_offset
                         , longitude
                         , latitude
                         , place_id
                         , created_by
                         , modified_by)
                    values (
                           @CommunityId
                         , @UserId
                         , @AddressTypeId
                         , @LotNumber
                         , @AddressLine1
                         , @AddressLine2
                         , @AddressLine3
                         , @City
                         , @StateCode
                         , @PostalCode
                         , @CountyCode
                         , @CountryCode
                         , @TimeZone
                         , @TimeZoneOffset
                         , @Longitude
                         , @Latitude
                         , @PlaceId
                         , @ModifiedBy
                         , @ModifiedBy)
                    returning Id
                    """, userAddress, cancellationToken: token));

            return addressId;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ErrorCodes.DatabaseError_AddUserAddress);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<Guid> AddCommunityAddress(CommunityAddress communityAddress, CancellationToken token = default)
    {
        try
        {
            using var connection = context.CreateConnection();
            var addressId = await connection.ExecuteScalarAsync<Guid>(
                new CommandDefinition(
                    """
                    insert into community_address (
                           community_id
                         , address_type_id
                         , lot_number
                         , address_1
                         , address_2
                         , address_3
                         , city
                         , state_code
                         , postal_code
                         , county_code
                         , country_code
                         , timezone
                         , timezone_offset
                         , longitude
                         , latitude
                         , place_id
                         , created_by
                         , modified_by)
                    values (
                           @CommunityId
                         , @AddressTypeId
                         , @LotNumber
                         , @AddressLine1
                         , @AddressLine2
                         , @AddressLine3
                         , @City
                         , @StateCode
                         , @PostalCode
                         , @CountyCode
                         , @CountryCode
                         , @TimeZone
                         , @TimeZoneOffset
                         , @Longitude
                         , @Latitude
                         , @PlaceId
                         , @ModifiedBy
                         , @ModifiedBy)
                    returning Id
                    """, communityAddress, cancellationToken: token));

            return addressId;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ErrorCodes.DatabaseError_AddCommunityAddress);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<bool> UpdateUserAddressAsync(UserAddress userAddress, CancellationToken token = default)
    {
        try
        {
            using var connection = context.CreateConnection();
            var recordsAffected = await connection.ExecuteAsync(
                new CommandDefinition(
                    """
                    update user_address 
                       set address_type_id = @AddressTypeId
                         , lot_number = @LotNumber
                         , address_1 = @AddressLine1
                         , address_2 = @AddressLine2
                         , address_3 = @AddressLine3
                         , city = @City
                         , state_code = @StateCode
                         , postal_code = @PostalCode
                         , county_code = @CountyCode
                         , country_code = @CountryCode
                         , timezone = @TimeZone
                         , timezone_offset = @TimeZoneOffset
                         , longitude = @Longitude
                         , latitude = @Latitude
                         , place_id = @PlaceId
                         , modified_by = @ModifiedBy
                         , modified_by = utcnow()
                     where id = @AddressId
                       and user_id = @UserId
                       and community_id = @CommunityId
                         
                    """, userAddress, cancellationToken: token));

            return recordsAffected == 1;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ErrorCodes.DatabaseError_UpdateUserAddress);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<bool> UpdateCommunityAddressAsync(CommunityAddress communityAddress,
        CancellationToken token = default)
    {
        try
        {
            using var connection = context.CreateConnection();
            var recordsAffected = await connection.ExecuteAsync(
                new CommandDefinition(
                    """
                    update user_address 
                       set address_type_id = @AddressTypeId
                         , lot_number = @LotNumber
                         , address_1 = @AddressLine1
                         , address_2 = @AddressLine2
                         , address_3 = @AddressLine3
                         , city = @City
                         , state_code = @StateCode
                         , postal_code = @PostalCode
                         , county_code = @CountyCode
                         , country_code = @CountryCode
                         , timezone = @TimeZone
                         , timezone_offset = @TimeZoneOffset
                         , longitude = @Longitude
                         , latitude = @Latitude
                         , place_id = @PlaceId
                         , modified_by = @ModifiedBy
                         , modified_by = utcnow()
                     where id = @AddressId
                       and community_id = @CommunityId
                         
                    """, communityAddress, cancellationToken: token));
            return recordsAffected == 1;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ErrorCodes.DatabaseError_UpdateCommunityAddress);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<UserAddress> GetUserAddressAsync(Guid addressId, Guid communityId, Guid userId,
        CancellationToken token = default)
    {
        try
        {
            using var connection = context.CreateConnection();
            var userAddress = await connection.QueryFirstOrDefaultAsync<UserAddress>(
                new CommandDefinition(
                    """
                    select *
                      from user_address
                     where id = @addressId
                       and community_id = @communityId
                       and user_id = @userId
                    """, new { addressId, communityId, userId }, cancellationToken: token));

            if (userAddress == null)
                throw new NotFoundException(string.Format(ErrorCodes.Address_NotFound_User, communityId, userId));

            return userAddress;
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                ex.GetType() == typeof(NotFoundException) ? ex.Message : ErrorCodes.DatabaseError_GetUserAddress);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<CommunityAddress> GetCommunityAddressAsync(Guid addressId, Guid communityId,
        CancellationToken token = default)
    {
        try
        {
            using var connection = context.CreateConnection();
            var communityAddress = await connection.QueryFirstOrDefaultAsync<CommunityAddress>(
                new CommandDefinition(
                    """
                    select *
                      from community_address
                     where id = @addressId
                       and community_id = @communityId
                    """, new { addressId, communityId }, cancellationToken: token));

            if (communityAddress == null)
                throw new NotFoundException(string.Format(ErrorCodes.Address_NotFound_Community, communityId));

            return communityAddress;
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                ex.GetType() == typeof(NotFoundException) ? ex.Message : ErrorCodes.DatabaseError_GetCommunityAddress);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<IEnumerable<UserAddress>> ListByUserAsync(Guid communityId, Guid userId,
        CancellationToken token = default)
    {
        try
        {
            using var connection = context.CreateConnection();
            var userAddresses = await connection.QueryAsync<UserAddress>(
                new CommandDefinition(
                    """
                    select *
                      from user_address
                     where community_id = @communityId
                       and user_id = @userId
                       and is_active
                    """, new { communityId, userId }, cancellationToken: token));

            return userAddresses;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ErrorCodes.DatabaseError_ListUserAddresses);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<IEnumerable<CommunityAddress>> ListByCommunityAsync(Guid communityId,
        CancellationToken token = default)
    {
        try
        {
            using var connection = context.CreateConnection();
            var communityAddresses = await connection.QueryAsync<CommunityAddress>(
                new CommandDefinition(
                    """
                    select *
                      from community_address
                     where community_id = @communityId
                       and is_active
                    """, communityId, cancellationToken: token));
            return communityAddresses;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ErrorCodes.DatabaseError_ListCommunityAddresses);
            throw;
        }
    }
}