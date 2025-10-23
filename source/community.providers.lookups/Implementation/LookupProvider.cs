using community.common.BaseClasses;
using community.data.entities.Lookups;
using community.data.postgres.Interfaces;
using community.models.Responses.Base;
using community.models.Responses.Lookups;
using community.providers.lookups.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace community.providers.lookups.Implementation;

/// <summary>
///     responsible for retrieving any related lookup data.
/// </summary>
/// <param name="lookupRepository">The data repository to request the lookup data from</param>
/// <param name="contextAccessor">The http context accessor</param>
/// <param name="logger">class logger</param>
public class LookupProvider(
    ILookupRepository lookupRepository,
    IHttpContextAccessor contextAccessor,
    ILogger<LookupProvider> logger)
    : BaseProvider(contextAccessor), ILookupProvider
{
    /// <inheritdoc />
    public async ValueTask<LookupResponse<AddressTypeResponse>> ListAddressTypesAsync()
    {
        var response = await MeasureExecutionAsync(async () =>
        {
            var addressTypes =
                (await lookupRepository.ListAsync<AddressType>(CurrentCommunityId))
                .Select(a => (AddressTypeResponse)a);

            return new LookupResponse<AddressTypeResponse>(addressTypes)
            {
                CorrelationId = CorrelationId
            };
        });
        logger.LogInformation(PrepareInformationLog(nameof(ListAddressTypesAsync), response.ExecutionMilliseconds));

        return response;
    }

    /// <inheritdoc />
    public async ValueTask<LookupResponse<ContactMethodResponse>> ListContactMethodsAsync()
    {
        var response = await MeasureExecutionAsync(async () =>
        {
            var contactMethods =
                (await lookupRepository.ListAsync<ContactMethod>(CurrentCommunityId)).Select(a =>
                    (ContactMethodResponse)a);

            return new LookupResponse<ContactMethodResponse>(contactMethods)
            {
                CorrelationId = CorrelationId
            };
        });

        logger.LogInformation(PrepareInformationLog(nameof(ListContactMethodsAsync), response.ExecutionMilliseconds));

        return response;
    }

    /// <inheritdoc />
    public async ValueTask<LookupResponse<ReportTypeResponse>> ListReportTypesAsync()
    {
        var response = await MeasureExecutionAsync(async () =>
        {
            var reportTypes =
                (await lookupRepository.ListAsync<ReportType>()).Select(a => (ReportTypeResponse)a);

            return new LookupResponse<ReportTypeResponse>(reportTypes)
            {
                CorrelationId = CorrelationId
            };
        });

        logger.LogInformation(PrepareInformationLog(nameof(ListReportTypesAsync), response.ExecutionMilliseconds));

        return response;
    }

    /// <inheritdoc />
    public async ValueTask<LookupResponse<UserTypeResponse>> ListUserTypesAsync()
    {
        var response = await MeasureExecutionAsync(async () =>
        {
            var userTypes =
                (await lookupRepository.ListAsync<UserType>())
                .Select(a => (UserTypeResponse)a);

            return new LookupResponse<UserTypeResponse>(userTypes)
            {
                CorrelationId = CorrelationId
            };
        });

        logger.LogInformation(PrepareInformationLog(nameof(ListUserTypesAsync), response.ExecutionMilliseconds));

        return response;
    }

    /// <inheritdoc />
    public async ValueTask<LookupResponse<ParcelSizeUnitResponse>> ListParcelSizeUnitsAsync()
    {
        var response = await MeasureExecutionAsync(async () =>
        {
            var parcelSizeUnits =
                (await lookupRepository.ListAsync<ParcelSizeUnit>()).Select(p => (ParcelSizeUnitResponse)p);
            return new LookupResponse<ParcelSizeUnitResponse>(parcelSizeUnits) { CorrelationId = CorrelationId };
        });
        logger.LogInformation(PrepareInformationLog(nameof(ListParcelSizeUnitsAsync), response.ExecutionMilliseconds));
        return response;
    }
}