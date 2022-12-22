using Data.Interceptors;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Data.Startup;

/// <summary>
/// Initializes the Data Access Layer using Entity Framework
/// </summary>
internal static class DataAccessDatabaseStartup
{
    public static string cDefaultDatabaseName = "CleanArchitectureDb";

    public static void Implement(IServiceCollection services, DataAccessStrategy strategy)
    {
        switch (strategy)
        {
            case DataAccessStrategy.Interceptor:
                ImplementWithInterceptors(services);
                break;
            case DataAccessStrategy.UnitOfWork:
                ImplementWithUnitOfWork(services);
                break;
            default:
                throw new NotImplementedException("A Data Access strategy was not selected or the selected strategy has not been implemented.");
        }
    }
    /// <summary>
    /// Setup the Data Access Layer using UnitOfWork.  
    /// </summary>
    /// <param name="services"></param>
    private static void ImplementWithUnitOfWork(IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.UseInMemoryDatabase(cDefaultDatabaseName);
        });
    }

    /// <summary>
    /// Setup the Data Access Layer using Interceptors
    /// </summary>
    /// <param name="services">the application service container</param>
    private static void ImplementWithInterceptors(IServiceCollection services)
    {
        services.AddSingleton<ConvertDomainEventsToOutboxMessagesInterceptor>();
        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            var interceptor = sp.GetService<ConvertDomainEventsToOutboxMessagesInterceptor>();
            if(interceptor is null)
                throw new ArgumentNullException(nameof(interceptor), "The interceptor could not be found.  Unable to initialize the database.  The application will not run properly.");

            options.UseInMemoryDatabase(cDefaultDatabaseName)
                .AddInterceptors(interceptor);
        });
    }
}
