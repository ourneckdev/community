using community.models.BusinessObjects;
using community.providers.common.Interfaces;

namespace community.providers.common.Implementation;

/// <summary>
///     Provider responsible for sending SMS notifications using Twilio
/// </summary>
public class SmsNotificationProvider : INotificationProvider
{
    /// <inheritdoc />
    public async Task SendNotificationAsync(Notification notification)
    {
        await Task.CompletedTask;
        throw new NotImplementedException();
    }
}