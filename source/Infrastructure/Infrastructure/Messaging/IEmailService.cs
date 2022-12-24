namespace Infrastructure.Messaging;
public interface IEmailService
{
    Task SendEmailNotificationAsync(string message, CancellationToken cancellationToken);
}
