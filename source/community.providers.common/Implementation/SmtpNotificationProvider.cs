using System.Net.Mail;
using community.models.BusinessObjects;
using community.providers.common.Interfaces;

namespace community.providers.common.Implementation;

/// <summary>
///     Sends notifications over SMTP
/// </summary>
public class SmtpNotificationProvider : INotificationProvider
{
    /// <inheritdoc cref="INotificationProvider.SendNotificationAsync" />
    public async Task SendNotificationAsync(Notification notification)
    {
        var client = new SmtpClient();
        var fromAddress = new MailAddress("donotreply@ourneck.com");

        foreach (var recipient in notification.Recipients)
        {
            var message = new MailMessage(fromAddress, new MailAddress(recipient));

            if (!string.IsNullOrWhiteSpace(notification.Subject))
                message.Subject = notification.Subject;
            message.IsBodyHtml = notification.SendAsEmail;
            message.Body = notification.Message;
            await client.SendMailAsync(message);
        }
    }
}