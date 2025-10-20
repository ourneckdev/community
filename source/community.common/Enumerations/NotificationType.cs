namespace community.common.Enumerations;

/// <summary>
///     Lists the available notification types.
/// </summary>
[Flags]
public enum NotificationType
{
    /// <summary>
    ///     Push notificaiton to mobile
    /// </summary>
    Push = 0x0,

    /// <summary>
    ///     Email notification
    /// </summary>
    Email = 0x1,

    /// <summary>
    ///     Sms notification
    /// </summary>
    Sms = 0x2,

    /// <summary>
    ///     No notifications.
    /// </summary>
    None = 0x4,
}