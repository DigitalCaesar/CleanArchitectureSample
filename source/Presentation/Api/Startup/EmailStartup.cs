using Infrastructure.Messaging;

namespace Api.Startup;

public static class EmailStartup
{
    public static IServiceCollection AddEmailService(this IServiceCollection services)
    {
        services.AddScoped<IEmailService, EmailService>();

        return services;
    }
}
