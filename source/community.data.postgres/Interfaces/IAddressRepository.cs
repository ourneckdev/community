using community.common.Interfaces;
using community.data.entities;

namespace community.data.postgres.Interfaces;

/// <summary>
///     Defines the methods available for manipulating address data.
/// </summary>
public interface IAddressRepository : IRepository
{
    /// <summary>
    ///     Adds a user address to the database.
    /// </summary>
    /// <param name="userAddress"></param>
    /// <param name="token"></param>
    /// <returns>The id of the newly created address record.</returns>
    Task<Guid> AddUserAddressAsync(UserAddress userAddress, CancellationToken token = default);

    /// <summary>
    ///     Adds a community address to the database.
    /// </summary>
    /// <param name="communityAddress"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    Task<Guid> AddCommunityAddress(CommunityAddress communityAddress, CancellationToken token = default);

    /// <summary>
    ///     Updates a user address.
    /// </summary>
    /// <param name="address"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    Task<bool> UpdateUserAddressAsync(UserAddress address, CancellationToken token = default);

    /// <summary>
    ///     Updates a community address
    /// </summary>
    /// <param name="communityAddress"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    Task<bool> UpdateCommunityAddressAsync(CommunityAddress communityAddress, CancellationToken token = default);

    /// <summary>
    ///     Retrieves a single user address by id.
    /// </summary>
    /// <param name="addressId">The id of the address record.</param>
    /// <param name="communityId">The community to which the address is assigned.</param>
    /// <param name="userId">The id of the user whose address is being retrieved.</param>
    /// <param name="token"></param>
    /// <returns></returns>
    Task<UserAddress> GetUserAddressAsync(Guid addressId, Guid communityId, Guid userId,
        CancellationToken token = default);

    /// <summary>
    ///     Retrieves a single community address by id.
    /// </summary>
    /// <param name="addressId">The id of the address record.</param>
    /// <param name="communityId">The community to which the address is assigned.</param>
    /// <param name="token"></param>
    /// <returns></returns>
    Task<CommunityAddress>
        GetCommunityAddressAsync(Guid addressId, Guid communityId, CancellationToken token = default);

    /// <summary>
    ///     Lists all the available active (not deleted) addresses for a user.
    /// </summary>
    /// <param name="communityId"></param>
    /// <param name="userId"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    Task<IEnumerable<UserAddress>> ListByUserAsync(Guid communityId, Guid userId, CancellationToken token = default);

    /// <summary>
    ///     Lists all the available active addresses for a community
    /// </summary>
    /// <param name="communityId"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    Task<IEnumerable<CommunityAddress>> ListByCommunityAsync(Guid communityId, CancellationToken token = default);
}