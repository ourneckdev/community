namespace community.models.BusinessObjects;

/// <summary>
///     Immutable Notification object encapsulating all the required data for handling SMS or Email Notifications.
/// </summary>
/// <param name="Recipients">The list of recipients receiving the notification.</param>
/// <param name="Subject">The optional subject of the notification.</param>
/// <param name="Message">The body of the message to send.</param>
/// <param name="SendAsEmail">A flag indicating if the notification body is html and should be sent with SMTP</param>
public record Notification(
    IEnumerable<string> Recipients,
    string? Subject,
    string Message,
    bool SendAsEmail);