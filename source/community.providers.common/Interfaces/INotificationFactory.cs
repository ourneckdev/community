using community.common.Interfaces;
using community.models.BusinessObjects;
using community.providers.common.Implementation;

namespace community.providers.common.Interfaces;

/// <summary>
///     Factory for instantiating the correct notification provider
/// </summary>
public interface INotificationFactory : IFactory
{
    /// <summary>
    ///     Instantiates the correct notification provider based on the provided notification
    /// </summary>
    /// <param name="notification">The notification to send.</param>
    /// <returns>
    ///     An instance of the <see cref="SmsNotificationProvider" />
    ///     or <see cref="SmtpNotificationProvider" /> based on where the message needs to be routed.
    /// </returns>
    INotificationProvider GetNotificationProvider(Notification notification);
}