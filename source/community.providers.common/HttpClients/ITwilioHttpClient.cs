using community.common.Interfaces;
using community.models.BusinessObjects;

namespace community.providers.common.HttpClients;

/// <summary>
/// Exposes endpoints for interacting with Twilio's service for sending SMS notifications. 
/// </summary>
public interface ITwilioHttpClient : IHttpClient
{
    /// <summary>
    /// Sets a notification to SMS via Twilio's rest client
    /// </summary>
    /// <param name="notification"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task SendMessageAsync(Notification notification, CancellationToken cancellationToken = default);
}