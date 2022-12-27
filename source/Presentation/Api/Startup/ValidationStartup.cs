using FluentValidation;

namespace Api.Startup;

public static class ValidationStartup
{
    public static IServiceCollection AddValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Application.AssemblyReference.Assembly, includeInternalTypes: true);

        return services;
    }
}
