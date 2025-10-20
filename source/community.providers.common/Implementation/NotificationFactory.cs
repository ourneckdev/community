using community.models.BusinessObjects;
using community.providers.common.Interfaces;

namespace community.providers.common.Implementation;

/// <summary>
/// Concrete implementation of the <see cref="INotificationFactory"/>
/// </summary>
public class NotificationFactory : INotificationFactory
{
    /// <inheritdoc />
    public INotificationProvider GetNotificationProvider(Notification notification) 
        => notification.SendAsEmail 
            ? new SmtpNotificationProvider() 
            : new SmsNotificationProvider();
}