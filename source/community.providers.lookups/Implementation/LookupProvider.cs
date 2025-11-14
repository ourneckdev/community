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
    private static IReadOnlyCollection<AddressTypeResponse>? _cachedAddressTypes;
    private static IReadOnlyCollection<ContactMethodResponse>? _cachedContactMethods;
    private static IReadOnlyCollection<ReportTypeResponse>? _cachedReportTypes;
    private static IReadOnlyCollection<UserTypeResponse>? _cachedUserTypes;
    private static IReadOnlyCollection<ParcelSizeUnitResponse>? _cachedParcelSizeUnit;

    /// <inheritdoc />
    public async ValueTask<LookupResponse<AddressTypeResponse>> ListAddressTypesAsync()
    {
        if (_cachedAddressTypes != null)
        {
            logger.LogInformation("Found address types in cache, returning.");
            return new LookupResponse<AddressTypeResponse>(_cachedAddressTypes) { CorrelationId = CorrelationId };
        }

        ;
        var response = await MeasureExecutionAsync(async () =>
        {
            var addressTypes = (await lookupRepository.ListAsync<AddressType>(CurrentCommunityId))
                .Select(a => (AddressTypeResponse)a)
                .ToList()
                .AsReadOnly();

            _cachedAddressTypes = addressTypes;
            return new LookupResponse<AddressTypeResponse>(addressTypes) { CorrelationId = CorrelationId };
        });
        logger.LogInformation(PrepareInformationLog(nameof(ListAddressTypesAsync), response.ExecutionMilliseconds));

        return response;
    }

    /// <inheritdoc />
    public async ValueTask<LookupResponse<ContactMethodResponse>> ListContactMethodsAsync()
    {
        if (_cachedContactMethods != null)
        {
            logger.LogInformation("Found address types in cache, returning.");
            return new LookupResponse<ContactMethodResponse>(_cachedContactMethods) { CorrelationId = CorrelationId };
        }

        var response = await MeasureExecutionAsync(async () =>
        {
            var contactMethods =
                (await lookupRepository.ListAsync<ContactMethod>(CurrentCommunityId)).Select(a => (ContactMethodResponse)a)
                .ToList()
                .AsReadOnly();

            _cachedContactMethods = contactMethods;
            return new LookupResponse<ContactMethodResponse>(contactMethods) { CorrelationId = CorrelationId };
        });

        logger.LogInformation(PrepareInformationLog(nameof(ListContactMethodsAsync), response.ExecutionMilliseconds));

        return response;
    }

    /// <inheritdoc />
    public async ValueTask<LookupResponse<ReportTypeResponse>> ListReportTypesAsync()
    {
        if (_cachedReportTypes != null)
        {
            logger.LogInformation("Found report types in cache, returning.");
            return new LookupResponse<ReportTypeResponse>(_cachedReportTypes) { CorrelationId = CorrelationId };
        }

        var response = await MeasureExecutionAsync(async () =>
        {
            var reportTypes =
                (await lookupRepository.ListAsync<ReportType>()).Select(a => (ReportTypeResponse)a)
                .ToList()
                .AsReadOnly();

            _cachedReportTypes = reportTypes;
            return new LookupResponse<ReportTypeResponse>(reportTypes) { CorrelationId = CorrelationId };
        });

        logger.LogInformation(PrepareInformationLog(nameof(ListReportTypesAsync), response.ExecutionMilliseconds));

        return response;
    }

    /// <inheritdoc />
    public async ValueTask<LookupResponse<UserTypeResponse>> ListUserTypesAsync()
    {
        if (_cachedUserTypes != null)
        {
            logger.LogInformation("Found user types in cache, returning.");
            return new LookupResponse<UserTypeResponse>(_cachedUserTypes) { CorrelationId = CorrelationId };
        }

        var response = await MeasureExecutionAsync(async () =>
        {
            var userTypes =
                (await lookupRepository.ListAsync<UserType>()).Select(a => (UserTypeResponse)a)
                .ToList()
                .AsReadOnly();

            _cachedUserTypes = userTypes;
            return new LookupResponse<UserTypeResponse>(userTypes) { CorrelationId = CorrelationId };
        });

        logger.LogInformation(PrepareInformationLog(nameof(ListUserTypesAsync), response.ExecutionMilliseconds));

        return response;
    }

    /// <inheritdoc />
    public async ValueTask<LookupResponse<ParcelSizeUnitResponse>> ListParcelSizeUnitsAsync()
    {
        if (_cachedParcelSizeUnit != null)
        {
            logger.LogInformation("Found report types in cache, returning.");
            return new LookupResponse<ParcelSizeUnitResponse>(_cachedParcelSizeUnit) { CorrelationId = CorrelationId };
        }

        var response = await MeasureExecutionAsync(async () =>
        {
            var parcelSizeUnits =
                (await lookupRepository.ListAsync<ParcelSizeUnit>()).Select(p => (ParcelSizeUnitResponse)p)
                .ToList()
                .AsReadOnly();
            
            _cachedParcelSizeUnit = parcelSizeUnits;
            return new LookupResponse<ParcelSizeUnitResponse>(parcelSizeUnits) { CorrelationId = CorrelationId };
        });
        logger.LogInformation(PrepareInformationLog(nameof(ListParcelSizeUnitsAsync), response.ExecutionMilliseconds));
        return response;
    }
}