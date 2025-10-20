using community.common.Interfaces;
using community.models.Abstract;
using community.models.Responses;
using community.models.Responses.Base;

namespace community.providers.community.Interfaces;

/// <summary>
///     Exposes methods for manipulation and retrieval of address data
/// </summary>
public interface IAddressProvider : IProvider
{
    /// <summary>
    ///     Sanitizes and saves a user's editAddress to the database
    /// </summary>
    /// <remarks>Integrates with GoogleSettings's GeoCode API to retrieve location information</remarks>
    /// <param name="editAddress">The input data to be sanitized and </param>
    /// <param name="cancellationToken">The optional cancelation token</param>
    /// <returns></returns>
    Task<SingleResponse<(Guid AddressId, bool Saved)>> SaveUserAddressAsync<T>(T editAddress,
        CancellationToken cancellationToken = default) where T : BaseAddressRequest;

    /// <summary>
    ///     Sanitizes and saves a community address to the database.
    /// </summary>
    /// <remarks>Integrates with GoogleSettings's GeoCode API to retrieve location information</remarks>
    /// <param name="editAddress"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<SingleResponse<(Guid AddressId, bool Saved)>> SaveCommunityAddressAsync<T>(T editAddress,
        CancellationToken cancellationToken = default) where T : BaseAddressRequest;

    /// <summary>
    ///     Retrieves a community address by id
    /// </summary>
    /// <param name="addressId">The id of the address record.</param>
    /// <param name="token">The cancellation token.</param>
    /// <returns></returns>
    Task<SingleResponse<CommunityAddressResponse>> GetCommunityAddressAsync(Guid addressId, CancellationToken token = default);

    /// <summary>
    ///     Retrieves a user address by id
    /// </summary>
    /// <param name="addressId">The id of the address record.</param>
    /// <param name="token">The cancellation token.</param>
    /// <returns></returns>
    Task<SingleResponse<UserAddressResponse>> GetUserAddressAsync(Guid addressId, CancellationToken token = default);

    /// <summary>
    /// Lists all the active addresses assigned to a community.
    /// </summary>
    /// <param name="communityId">The community id to retrieve the addresses against</param>
    /// <param name="token">The cancellation token.</param>
    /// <returns></returns>
    Task<MultiResponse<CommunityAddressResponse>> ListCommunityAddressesAsync(Guid communityId,
        CancellationToken token = default);

    /// <summary>
    /// Lists all the available addresses assigned to a community user.
    /// </summary>
    /// <param name="communityId"></param>
    /// <param name="userId"></param>
    /// <param name="token">The cancellation token.</param>
    /// <returns></returns>
    Task<MultiResponse<UserAddressResponse>> ListUserAddressesAsync(Guid communityId, Guid userId,
        CancellationToken token = default);
}