using Infrastructure.Authentication;
using Microsoft.Extensions.Options;

namespace Api.Options;

public class JwtOptionsSetup : IConfigureOptions<JwtOptions>
{
    private readonly IConfiguration mConfiguration;
    private const string cSectionName = "Jwt";

    public JwtOptionsSetup(IConfiguration configuration)
    {
        mConfiguration = configuration;
    }

    public void Configure(JwtOptions options)
    {
        mConfiguration.GetSection(cSectionName).Bind(options);
    }
}
