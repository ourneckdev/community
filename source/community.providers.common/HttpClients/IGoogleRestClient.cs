using community.common.Interfaces;
using community.models.BusinessObjects.Google.Geocode;

namespace community.providers.common.HttpClients;

/// <summary>
/// Manages the http client interacting with GoogleSettings APIs. 
/// </summary>
public interface IGoogleRestClient : IHttpClient
{
    /// <summary>
    /// Retrieves the GoogleSettings GeoCoding information for a supplied address.
    /// </summary>
    /// <param name="address">The stringified address to retrieve location data for.  AddressModel is url encoded.</param>
    /// <param name="cancellationToken">The optional cancellation token to pass</param>
    /// <returns>A hydrated <see cref="Response"/> object with the potential matches.</returns>
    Task<Response?> GetGecodeForAddressAsync(string address, CancellationToken cancellationToken = default);
}