using System.Diagnostics.CodeAnalysis;
using community.common.AppSettings;
using community.models.BusinessObjects.Google.Geocode;
using community.providers.common.HttpClients;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace community.providers.common.tests;

[ExcludeFromCodeCoverage]
public class GoogleRestClientTests
{
    private static readonly HttpClient HttpClient = new();
    private static readonly GoogleSettings GoogleSettings;

    private readonly GoogleRestClient _googleRestClient = new(HttpClient, Options.Create(GoogleSettings));

    static GoogleRestClientTests()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddUserSecrets<GoogleRestClientTests>()
            .AddJsonFile("appsettings.json", false, true)
            .Build();


        GoogleSettings = configuration.GetSection(nameof(community.common.AppSettings.GoogleSettings))
                             .Get<GoogleSettings>()
                         ?? throw new ArgumentNullException(
                             $"{nameof(community.common.AppSettings.GoogleSettings)} configuration not provided.");
        HttpClient.BaseAddress = new Uri(GoogleSettings.GeoCodeBaseUrl);
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task TestAddressLookup_ReturnsValue_ShouldSucceed()
    {
        var streetAddress = "33110 Fishers Peak Pkwy";
        var city = "Trinidad";
        var state = "CO";
        var postalCode = "81082";
        var response = await _googleRestClient.GetGecodeForAddressAsync($"{streetAddress} {city} {state} {postalCode}");
        Assert.NotNull(response);
        Assert.IsType<Response>(response);

        var resolvedStreetAddress = response.Results.FirstOrDefault()?.GetStreetAddress();
        Assert.NotNull(resolvedStreetAddress);
        Assert.Equal(city, response.Results.FirstOrDefault()?.GetComponent(ComponentType.locality)?.LongName);
        Assert.Equal(state,
            response.Results.FirstOrDefault()?.GetComponent(ComponentType.administrative_area_level_1)?.ShortName);
        Assert.StartsWith(postalCode, response.Results.FirstOrDefault()?.GetPostalCode());
    }
}