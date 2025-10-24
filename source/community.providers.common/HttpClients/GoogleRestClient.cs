using System.Net.Http.Json;
using System.Text.Json;
using System.Web;
using community.common.AppSettings;
using community.models.BusinessObjects.Google.Geocode;
using Microsoft.Extensions.Options;

namespace community.providers.common.HttpClients;

/// <inheritdoc />
public class GoogleRestClient : IGoogleRestClient
{
    private readonly HttpClient _client;
    private readonly GoogleSettings _googleSettings;
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    /// <summary>
    ///     Instantiates the new client
    /// </summary>
    /// <param name="client"></param>
    /// <param name="googleSettings"></param>
    public GoogleRestClient(HttpClient client, IOptions<GoogleSettings> googleSettings)
    {
        _client = client;
        _googleSettings = googleSettings.Value;
        _jsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    /// <inheritdoc />
    public async Task<Response?> GetGecodeForAddressAsync(string address, CancellationToken cancellationToken = default)
    {
        var url = $"?key={_googleSettings.ApiKey}&address={HttpUtility.UrlEncode(address)}";
        using var response = await _client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        response.EnsureSuccessStatusCode();
        var parsedResponse =
            await response.Content.ReadFromJsonAsync<Response>(_jsonSerializerOptions, cancellationToken);
        return parsedResponse;
    }
}