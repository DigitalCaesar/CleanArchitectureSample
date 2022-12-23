using Data.Options;
using Microsoft.Extensions.Options;

namespace Api.Options;
public class DatabaseOptionsSetup : IConfigureOptions<DatabaseOptions>
{
    private const string DatabaseName = "CleanArchitectureSample";
    private const string ConfigurationSectionName = "DatabaseOptions";
    private readonly IConfiguration mConfiguration;

    public DatabaseOptionsSetup(IConfiguration configuration)
    {
        mConfiguration = configuration;
    }
    public void Configure(DatabaseOptions options)
    {
        options.ConnectionString = mConfiguration.GetConnectionString(DatabaseName) ?? DatabaseName;

        mConfiguration.GetSection(ConfigurationSectionName).Bind(options);
    }
}
