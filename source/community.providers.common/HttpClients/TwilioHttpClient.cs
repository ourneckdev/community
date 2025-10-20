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
public class TwilioHttpClient : ITwilioHttpClient
{
    private const string Sender = "";
    
    private readonly ITwilioRestClient _twilioRestClient;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public Response Request(Request request) => _twilioRestClient.Request(request);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public Task<Response> RequestAsync(Request request) => _twilioRestClient.RequestAsync(request);
    /// <summary>
    /// 
    /// </summary>
    public string AccountSid => _twilioRestClient.AccountSid;
    /// <summary>
    /// 
    /// </summary>
    public string Region => _twilioRestClient.Region;
    /// <summary>
    /// 
    /// </summary>
    public Twilio.Http.HttpClient HttpClient => _twilioRestClient.HttpClient;

    /// <summary>
    /// Intializes the client
    /// </summary>
    /// <param name="client"></param>
    /// <param name="options"></param>
    public TwilioHttpClient(HttpClient client, IOptions<TwilioSettings> options)
    {
        client.DefaultRequestHeaders.Add("X-Custom-Header", "CustomTwilioRestClient-Demo");

        var twilioSettings = options.Value;
        _twilioRestClient = new TwilioRestClient(twilioSettings.AccountSid, twilioSettings.AuthToken, httpClient: new SystemNetHttpClient(client));
    }

    /// <inheritdoc />
    public async Task SendMessageAsync(Notification notification, CancellationToken cancellationToken = default)
    {
        foreach(var recipient in notification.Recipients)
        {
            var message = await MessageResource.CreateAsync(
                to: new PhoneNumber(recipient),
                from: new PhoneNumber(Sender),
                body: notification.Message);
        }
    }
}
