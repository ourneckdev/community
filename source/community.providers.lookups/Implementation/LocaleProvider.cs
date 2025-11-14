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
    private static IReadOnlyCollection<CountryResponse>? _cachedCountries;
    private static readonly Dictionary<string, IReadOnlyCollection<StateResponse>> CachedStatesByCountry = new();
    private static readonly Dictionary<string, IReadOnlyCollection<CountyResponse>> CachedCountiesByState = new();
    private static readonly Dictionary<string, IReadOnlyCollection<TimeZoneResponse>> CachedTimeZones = new();

    /// <inheritdoc />
    public async ValueTask<LookupResponse<CountryResponse>> ListCountriesAsync()
    {
        if (_cachedCountries != null)
        {
            logger.LogInformation("Found countries in cache, returning.");
            return new LookupResponse<CountryResponse>(_cachedCountries) { CorrelationId = CorrelationId };
        }

        var response = await MeasureExecutionAsync(async () =>
        {
            var countries = (await localeRepository.ListCountriesAsync()).Select(c => (CountryResponse)c)
                .ToList()
                .AsReadOnly();

            if(countries.Count != 0)
                _cachedCountries = countries;
            
            var lookupResponse = new LookupResponse<CountryResponse>(countries) { CorrelationId = CorrelationId };
            return lookupResponse;
        });

        logger.LogInformation(PrepareInformationLog(nameof(ListCountriesAsync), response.ExecutionMilliseconds));
        return response;
    }

    /// <inheritdoc cref="ILocaleProvider.ListStatesAsync" />
    public async ValueTask<LookupResponse<StateResponse>> ListStatesAsync(string countryCode)
    {
        if (CachedStatesByCountry.TryGetValue(countryCode, out var cachedStates))
        {
            logger.LogInformation("Found cached states for country code {countryCode}", countryCode);
            return new LookupResponse<StateResponse>(cachedStates) { CorrelationId = CorrelationId };
        }

        var response = await MeasureExecutionAsync(async () =>
        {
            var states = (await localeRepository.ListStatesAsync(countryCode)).Select(c => (StateResponse)c).ToList().AsReadOnly();
            
            if(states.Count != 0)
                CachedStatesByCountry.TryAdd(countryCode, states);
            
            return new LookupResponse<StateResponse>(states) { CorrelationId = CorrelationId };
        });

        logger.LogInformation(PrepareInformationLog(nameof(ListCountriesAsync), response.ExecutionMilliseconds));

        return response;
    }

    /// <inheritdoc cref="ILocaleProvider.ListCountiesAsync" />
    public async ValueTask<LookupResponse<CountyResponse>> ListCountiesAsync(string countryCode, string stateCode)
    {
        if (CachedCountiesByState.TryGetValue(stateCode, out var cachedCounties))
            return new LookupResponse<CountyResponse>(cachedCounties) { CorrelationId = CorrelationId };
        ;

        var response = await MeasureExecutionAsync(async () =>
        {
            var counties = (await localeRepository.ListCountiesAsync(countryCode, stateCode)).Select(c => (CountyResponse)c)
                .ToList()
                .AsReadOnly();

            if(counties.Count != 0)
                CachedCountiesByState.TryAdd(stateCode, counties);
            return new LookupResponse<CountyResponse>(counties) { CorrelationId = CorrelationId };
        });

        logger.LogInformation(PrepareInformationLog(nameof(ListCountiesAsync), response.ExecutionMilliseconds));

        return response;
    }

    /// <inheritdoc />
    public async ValueTask<LookupResponse<TimeZoneResponse>> ListTimeZonesAsync(string countryCode)
    {
        if (CachedTimeZones.TryGetValue(countryCode, out var cachedTimeZones))
            return new LookupResponse<TimeZoneResponse>(cachedTimeZones) { CorrelationId = CorrelationId };

        var response = await MeasureExecutionAsync(async () =>
        {
            var timezones
                = (await localeRepository.ListTimeZonesAsync(countryCode)).Select(c => (TimeZoneResponse)c)
                .ToList()
                .AsReadOnly();
            
            if(timezones.Count != 0)
                CachedTimeZones.TryAdd(countryCode, timezones);
            
            return new LookupResponse<TimeZoneResponse>(timezones) { CorrelationId = CorrelationId };
        });

        logger.LogInformation(PrepareInformationLog(nameof(ListCountiesAsync), response.ExecutionMilliseconds));

        return response;
    }
}