using Data.Interceptors;
using Data.Options;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Data.Startup;

/// <summary>
/// Initializes the Data Access Layer using Entity Framework
/// </summary>
internal static class DataAccessDatabaseStartup
{
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
            case DataAccessStrategy.None:
            default:
                ImplementDefault(services);
                break;
        }
    }
    /// <summary>
    /// Setup the Data Access Layer using UnitOfWork.  
    /// </summary>
    /// <param name="services"></param>
    private static void ImplementDefault(IServiceCollection services)
    {
        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            var databaseOptions = sp.GetService<IOptions<DatabaseOptions>>()!.Value;
            options.UseInMemoryDatabase(databaseOptions.DatabaseName);
            options.EnableDetailedErrors(databaseOptions.EnableDetailedErrors);
            options.EnableSensitiveDataLogging(databaseOptions.EnableSensitiveDataLogging);
        });
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
            var databaseOptions = sp.GetService<IOptions<DatabaseOptions>>()!.Value;
            options.UseInMemoryDatabase(databaseOptions.DatabaseName);
            options.EnableDetailedErrors(databaseOptions.EnableDetailedErrors);
            options.EnableSensitiveDataLogging(databaseOptions.EnableSensitiveDataLogging);
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
            var databaseOptions = sp.GetService<IOptions<DatabaseOptions>>()!.Value;
            var interceptor = sp.GetService<ConvertDomainEventsToOutboxMessagesInterceptor>();
            if(interceptor is null)
                throw new ArgumentNullException(nameof(interceptor), "The interceptor could not be found.  Unable to initialize the database.  The application will not run properly.");

            options.UseInMemoryDatabase(databaseOptions.DatabaseName)
                .AddInterceptors(interceptor);
            options.EnableDetailedErrors(databaseOptions.EnableDetailedErrors);
            options.EnableSensitiveDataLogging(databaseOptions.EnableSensitiveDataLogging);
        });
    }
}
