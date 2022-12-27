using Microsoft.Extensions.DependencyInjection;
using MediatR;
using Infrastructure.Idempotent;
using Application.Behaviors;

namespace Api.Startup;

public static class MediatorStartup
{
    public static IServiceCollection AddMediator(this IServiceCollection services)
    {
        services.AddMediatR(Application.AssemblyReference.Assembly);
        services.AddMediatR(Api.AssemblyReference.Assembly);
        services.Decorate(typeof(INotificationHandler<>), typeof(IdempotentDomainEventHandler<>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        return services;
    }
}
