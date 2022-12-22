using Microsoft.Extensions.DependencyInjection;
using MediatR;

namespace Api.Startup;

public static class MediatorStartup
{
    public static void AddMediator(this ServiceCollection services)
    {
        services.AddMediatR(Application.AssemblyReference.Assembly);
        services.AddMediatR(Api.AssemblyReference.Assembly);
    }
}
