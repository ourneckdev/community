using community.common.BaseClasses;
using community.common.Definitions;
using community.data.postgres.Interfaces;
using community.models.Abstract;
using community.models.Responses;
using community.models.Responses.Base;
using community.providers.common.HttpClients;
using community.providers.community.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace community.providers.community.Implementation;

/// <summary>
///     Exposes methods related to saving address data for users and communities
/// </summary>
/// <param name="addressRepository">The repository to interact with address data.</param>
/// <param name="googleRestClient">The custom GoogleSettings client meant for interacting with GoogleSettings APIs</param>
/// <param name="contextAccessor">The context accessor, required for understanding the request</param>
/// <param name="logger">The class logger.</param>
public class AddressProvider(
    IAddressRepository addressRepository,
    IGoogleRestClient googleRestClient,
    IHttpContextAccessor contextAccessor,
    ILogger<AddressProvider> logger)
    : BaseProvider(contextAccessor), IAddressProvider
{
    /// <inheritdoc />
    public async Task<SingleResponse<(Guid AddressId, bool Saved)>> SaveUserAddressAsync<T>(
        T address,
        CancellationToken cancellationToken = default)
        where T : BaseAddressRequest
    {
        var response = await MeasureExecutionAsync(async () =>
        {
            var addressId = address.Id.GetValueOrDefault();
            bool saved;

            await PopulateGeoCodeData(address, cancellationToken);

            if (address.Id == Guid.Empty)
            {
                addressId = await addressRepository.AddUserAddressAsync(address.AddUserAudit(UserId).ToUserAddress(), cancellationToken);
                saved = true;
            }
            else
            {
                saved = await addressRepository.UpdateUserAddressAsync(address.AddUserAudit(UserId).ToUserAddress(), cancellationToken);
            }

            return new SingleResponse<(Guid AddressId, bool Saved)>((addressId, saved));
        });
        logger.LogInformation(
            PrepareInformationLog(nameof(SaveUserAddressAsync), response.ExecutionMilliseconds));

        return response;
    }

    /// <inheritdoc />
    public async Task<SingleResponse<(Guid AddressId, bool Saved)>> SaveCommunityAddressAsync<T>(
        T address,
        CancellationToken cancellationToken = default)
        where T : BaseAddressRequest
    {
        var response = await MeasureExecutionAsync(async () =>
        {
            var addressId = address.Id.GetValueOrDefault();
            bool saved;
            await PopulateGeoCodeData(address, cancellationToken);

            if (address.Id == Guid.Empty)
            {
                addressId = await addressRepository.AddCommunityAddress(address.AddUserAudit(UserId).ToCommunityAddress(),
                    cancellationToken);
                saved = true;
            }
            else
            {
                saved = await addressRepository.UpdateCommunityAddressAsync(address.AddUserAudit(UserId).ToCommunityAddress(),
                    cancellationToken);
            }

            return new SingleResponse<(Guid AddressId, bool Saved)>((addressId, saved));
        });
        logger.LogInformation(PrepareInformationLog(nameof(SaveCommunityAddressAsync), response.ExecutionMilliseconds));

        return response;
    }

    /// <inheritdoc />
    public async Task<SingleResponse<CommunityAddressResponse>> GetCommunityAddressAsync(Guid addressId, CancellationToken token = default)
    {
        var response = await MeasureExecutionAsync(async () =>
        {
            var userAddress = await addressRepository.GetCommunityAddressAsync(addressId, CurrentCommunityId.GetValueOrDefault(), token);
            return new SingleResponse<CommunityAddressResponse>(userAddress);
        });

        logger.LogInformation(PrepareInformationLog($"{nameof(GetUserAddressAsync)} with UserAddressResponse", response.ExecutionMilliseconds));

        return response;
    }

    /// <inheritdoc />
    public async Task<SingleResponse<UserAddressResponse>> GetUserAddressAsync(Guid addressId, CancellationToken token = default)
    {
        var response = await MeasureExecutionAsync(async () =>
        {
            var userAddress = await addressRepository.GetUserAddressAsync(addressId, CurrentCommunityId.GetValueOrDefault(), UserId.GetValueOrDefault(), token);
            return new SingleResponse<UserAddressResponse>(userAddress);
        });

        logger.LogInformation(PrepareInformationLog($"{nameof(GetUserAddressAsync)} with UserAddressResponse", response.ExecutionMilliseconds));

        return response;
    }

    /// <inheritdoc />
    public async Task<MultiResponse<CommunityAddressResponse>> ListCommunityAddressesAsync(Guid communityId, CancellationToken token = default)
    {
        var response = await MeasureExecutionAsync(async () =>
        {
            var communityAddresses =
                await addressRepository.ListByCommunityAsync(communityId, token);
            return new MultiResponse<CommunityAddressResponse>(communityAddresses.Select(c => (CommunityAddressResponse)c));
        });

        logger.LogInformation(PrepareInformationLog($"{nameof(ListCommunityAddressesAsync)}",
            response.ExecutionMilliseconds));

        return response;
    }

    /// <inheritdoc />
    public async Task<MultiResponse<UserAddressResponse>> ListUserAddressesAsync(Guid communityId, Guid userId, CancellationToken token = default)
    {
        var response = await MeasureExecutionAsync(async () =>
        {
            var userAddresses =
                await addressRepository.ListByUserAsync(communityId, userId, token);
            return new MultiResponse<UserAddressResponse>(userAddresses.Select(c => (UserAddressResponse)c));
        });

        logger.LogInformation(PrepareInformationLog($"{nameof(ListCommunityAddressesAsync)}",
            response.ExecutionMilliseconds));

        return response;
    }


    private async Task PopulateGeoCodeData(BaseAddressRequest editAddress,
        CancellationToken cancellationToken = default)
    {
        // validate the address before we make the call to google.  if it fails validation, throw error.
        editAddress.Validate();
        try
        {
            var locationData = (await googleRestClient.GetGecodeForAddressAsync(editAddress.ToString(), cancellationToken))
                ?.Results.FirstOrDefault();
            editAddress.Latitude = locationData?.Geometry?.Location?.Latitude;
            editAddress.Longitude = locationData?.Geometry?.Location?.Longitude;
            editAddress.PlaceId = locationData?.PlaceId;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ErrorCodes.Google_FailedToReturnGeoCodeData);
        }
    }
}