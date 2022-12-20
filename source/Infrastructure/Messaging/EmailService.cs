using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging;
public class EmailService : IEmailService
{
    public Task SendEmailNotificationAsync(string message, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Simulating email message send of message: {message}");
        return Task.CompletedTask;
    }
}
