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
        services.AddMediatR(Application.AssemblyReference.Assembly);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddFluentValidationAutoValidation();//.AddValidatorsFromAssembly(applicationAssembly);
    }
}
