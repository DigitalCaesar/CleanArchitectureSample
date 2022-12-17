using MediatR;
using FluentValidation;
using Application.Behaviors;
using FluentValidation.AspNetCore;

namespace Api;

public class Startup
{
    public IConfiguration Configuration { get; }
    public Startup(IConfiguration configuration)
    { 
        Configuration = configuration; 
    }

    public void ConfigureServices(IServiceCollection services)
    {
        var presenationAssembly = typeof(Api.AssemblyReference).Assembly;
        var applicationAssembly = typeof(Application.AssemblyReference).Assembly;
        services.AddMediatR(applicationAssembly);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddFluentValidationAutoValidation();//.AddValidatorsFromAssembly(applicationAssembly);
    }
}
