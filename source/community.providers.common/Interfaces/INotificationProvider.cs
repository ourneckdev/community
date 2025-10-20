using community.common.Interfaces;
using community.models.BusinessObjects;

namespace community.providers.common.Interfaces;

/// <summary>
///     Defines available methods for sending notifications.
/// </summary>
public interface INotificationProvider : IProvider
{
    /// <inheritdoc cref="INotificationProvider.SendNotificationAsync" />
    Task SendNotificationAsync(Notification notification);
}