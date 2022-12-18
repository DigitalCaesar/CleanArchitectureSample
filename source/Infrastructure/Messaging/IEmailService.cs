using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging;
public interface IEmailService
{
    Task SendEmailNotificationAsync(string message, CancellationToken cancellationToken);
}
