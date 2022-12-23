using Data.Startup;

namespace Data.Options;
public class DatabaseOptions
{
    public string DatabaseName { get; set; } = "Default";
    public int MaxRetryCount { get; set; } = 3;
    public int CommandTimeout { get; set; } = 60;
    public bool EnableDetailedErrors { get; set; } = false;
    public bool EnableSensitiveDataLogging { get; set; } = false;
    public string ConnectionString { get; set; } = string.Empty;
    public CachingInitializationStrategy CachingStrategy { get; set; } = CachingInitializationStrategy.None;
    public DataAccessStrategy DataAccessStrategy { get; set; } = DataAccessStrategy.None;
}
