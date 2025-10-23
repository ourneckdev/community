using System.ComponentModel;
using community.common.AppSettings;
using community.models.BusinessObjects.Google.Geocode;
using community.providers.common.HttpClients;
using Microsoft.Extensions.Options;

namespace community.providers.common.tests;

public class GoogleRestClientTests
{
    private static readonly HttpClient HttpClient = new();

    private readonly GoogleRestClient _googleRestClient = new(HttpClient, Options.Create(new GoogleSettings
    {
        GeoCodeBaseUrl = "https://maps.googleapis.com/maps/api/geocode/json",
    }));


    [Fact]
    [Category("Integration")]
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