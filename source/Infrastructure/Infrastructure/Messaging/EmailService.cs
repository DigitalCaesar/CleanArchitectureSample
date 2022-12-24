namespace Infrastructure.Messaging;
public class EmailService : IEmailService
{
    public Task SendEmailNotificationAsync(string message, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Simulating email message send of message: {message}");
        return Task.CompletedTask;
    }
}
