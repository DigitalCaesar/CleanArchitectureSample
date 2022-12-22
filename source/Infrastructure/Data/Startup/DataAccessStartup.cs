using Microsoft.Extensions.DependencyInjection;

namespace Data.Startup;

/// <summary>
/// Initializes the Data Access Layer using Entity Framework
/// </summary>
public static class DataAccessStartup
{
    public static void AddDataAccessEntityFramework(this IServiceCollection services, 
        DataAccessStrategy dataStrategy, 
        CachingInitializationStrategy cacheStrategy)
    {
        DataAccessDatabaseStartup.Implement(services, dataStrategy);
        DataAccessRepositoryStartup.Register(services, cacheStrategy);
    }

}
