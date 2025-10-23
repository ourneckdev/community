using community.common.BaseClasses;
using community.data.postgres.Interfaces;
using community.models.Responses.Base;
using community.models.Responses.Locales;
using community.providers.lookups.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace community.providers.lookups.Implementation;

/// <summary>
///     Retrieves local specific lookup data
/// </summary>
/// <param name="localeRepository"></param>
/// <param name="contextAccessor"></param>
/// <param name="logger"></param>
public class LocaleProvider(
    ILocaleRepository localeRepository,
    IHttpContextAccessor contextAccessor,
    ILogger<LocaleProvider> logger)
    : BaseProvider(contextAccessor), ILocaleProvider
{
    /// <inheritdoc />
    public async ValueTask<LookupResponse<CountryResponse>> ListCountriesAsync()
    {
        var response = await MeasureExecutionAsync(async () =>
        {
            var countries
                = (await localeRepository.ListCountriesAsync()).Select(c => (CountryResponse)c);
            return new LookupResponse<CountryResponse>(countries)
            {
                CorrelationId = CorrelationId
            };
        });

        logger.LogInformation(PrepareInformationLog(nameof(ListCountriesAsync), response.ExecutionMilliseconds));

        return response;
    }

    /// <inheritdoc cref="ILocaleProvider.ListStatesAsync" />
    public async ValueTask<LookupResponse<StateResponse>> ListStatesAsync(string countryCode)
    {
        var response = await MeasureExecutionAsync(async () =>
        {
            var states
                = (await localeRepository.ListStatesAsync(countryCode)).Select(c => (StateResponse)c);
            return new LookupResponse<StateResponse>(states)
            {
                CorrelationId = CorrelationId
            };
        });

        logger.LogInformation(PrepareInformationLog(nameof(ListCountriesAsync), response.ExecutionMilliseconds));

        return response;
    }

    /// <inheritdoc cref="ILocaleProvider.ListCountiesAsync" />
    public async ValueTask<LookupResponse<CountyResponse>> ListCountiesAsync(string countryCode, string stateCode)
    {
        var response = await MeasureExecutionAsync(async () =>
        {
            var counties
                = (await localeRepository.ListCountiesAsync(countryCode, stateCode)).Select(c => (CountyResponse)c);

            return new LookupResponse<CountyResponse>(counties)
            {
                CorrelationId = CorrelationId
            };
        });

        logger.LogInformation(PrepareInformationLog(nameof(ListCountiesAsync), response.ExecutionMilliseconds));

        return response;
    }

    /// <inheritdoc />
    public async ValueTask<LookupResponse<TimeZoneResponse>> ListTimeZonesAsync(string countryCode)
    {
        var response = await MeasureExecutionAsync(async () =>
        {
            var timezones
                = (await localeRepository.ListTimeZonesAsync(countryCode)).Select(c => (TimeZoneResponse)c);

            return new LookupResponse<TimeZoneResponse>(timezones)
            {
                CorrelationId = CorrelationId
            };
        });

        logger.LogInformation(PrepareInformationLog(nameof(ListCountiesAsync), response.ExecutionMilliseconds));

        return response;
    }
}