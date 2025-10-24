using community.common.AppSettings;
using community.models.BusinessObjects;
using Microsoft.Extensions.Options;
using Twilio.Clients;
using Twilio.Http;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using HttpClient = System.Net.Http.HttpClient;

namespace community.providers.common.HttpClients;

/// <inheritdoc />
[Obsolete("Twillio account deactivated")]
public class TwilioHttpClient : ITwilioHttpClient
{
    private const string Sender = "";

    private readonly ITwilioRestClient _twilioRestClient;

    /// <summary>
    ///     Intializes the client
    /// </summary>
    /// <param name="client"></param>
    /// <param name="options"></param>
    public TwilioHttpClient(HttpClient client, IOptions<TwilioSettings> options)
    {
        client.DefaultRequestHeaders.Add("X-Custom-Header", "CustomTwilioRestClient-Demo");

        var twilioSettings = options.Value;
        _twilioRestClient = new TwilioRestClient(twilioSettings.AccountSid, twilioSettings.AuthToken,
            httpClient: new SystemNetHttpClient(client));
    }

    /// <summary>
    /// </summary>
    public string AccountSid => _twilioRestClient.AccountSid;

    /// <summary>
    /// </summary>
    public string Region => _twilioRestClient.Region;

    /// <summary>
    /// </summary>
    public Twilio.Http.HttpClient HttpClient => _twilioRestClient.HttpClient;

    /// <inheritdoc />
    public async Task SendMessageAsync(Notification notification, CancellationToken cancellationToken = default)
    {
        foreach (var recipient in notification.Recipients)
        {
            var message = await MessageResource.CreateAsync(
                new PhoneNumber(recipient),
                from: new PhoneNumber(Sender),
                body: notification.Message);
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public Response Request(Request request)
    {
        return _twilioRestClient.Request(request);
    }

    /// <summary>
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public Task<Response> RequestAsync(Request request)
    {
        return _twilioRestClient.RequestAsync(request);
    }
}