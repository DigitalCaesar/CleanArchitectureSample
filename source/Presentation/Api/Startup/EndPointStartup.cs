using Api.Controllers;
using DigitalCaesar.Server.Api;

namespace Api.Startup;

public static class EndPointStartup
{
    public static IServiceCollection AddEndPoints(this IServiceCollection services)
    {
        services.AddEndpointDefinitions(typeof(MemberController));
        services.AddEndpointsApiExplorer();

        return services;
    }

    public static WebApplication UseEndPoints(this WebApplication app)
    {
        app.UseEndpointDefinitions();
        app.UseHttpsRedirection();

        return app;
    }
}
