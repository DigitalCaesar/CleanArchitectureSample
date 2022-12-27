using Api.Middleware;

namespace Api.Startup;

public static class ExceptionHandlingStartup
{
    public static IServiceCollection AddExceptionHandling(this IServiceCollection services)
    {
        services.AddTransient<ExceptionHandlingMiddleware>();

        return services; 
    }

    public static WebApplication UseExceptionHandling(this WebApplication app)
    {
        //app.UseMiddleware<ExceptionHandlingMiddlewareSimple>();
        app.UseMiddleware<ExceptionHandlingMiddleware>();

        return app;
    }
}
