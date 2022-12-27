using Data.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Diagnostics.Tracing;

namespace Data.Startup;

/// <summary>
/// Initializes the Data Access Layer using Entity Framework
/// </summary>
public static class DataAccessStartup
{
    public static IServiceCollection AddDataAccessEntityFramework(this IServiceCollection services,
        IConfiguration configuration)
    {
        //TODO:  Is there a way to do this with IOptions???
        DataAccessStrategy dataAccessStrategy = Enum.Parse<DataAccessStrategy>(configuration.GetSection("DatabaseOptions:DataAccessStrategy").Value ?? "None");
        CachingInitializationStrategy cachingStrategy = Enum.Parse<CachingInitializationStrategy>(configuration.GetSection("DatabaseOptions:CachingStrategy").Value ?? "None");
        DataAccessDatabaseStartup.Implement(services, dataAccessStrategy);
        DataAccessRepositoryStartup.Register(services, cachingStrategy);

        return services;
    }

}
