using Api.Options;
using Infrastructure.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace Api.Startup;

public static class SecurityStartup
{
    public static IServiceCollection AddSecurity(this IServiceCollection services)
    {
        services.AddScoped<IJwtProvider, JwtProvider>();
        services.ConfigureOptions<JwtOptionsSetup>();
        services.ConfigureOptions<JwtBearerOptionsSetup>();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer();
        //    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));
        services.AddAuthorization();
        services.AddAuthentication();
        services.AddScoped<IPermissionService, PermissionService>();
        services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();

        return services;
    }
    public static WebApplication UseSecurity(this WebApplication app)
    {
        app.UseAuthentication();
        app.UseAuthorization();

        return app;
    }
}
