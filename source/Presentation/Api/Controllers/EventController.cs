using Data.Repositories;
using DigitalCaesar.Server.Api;
using Domain.Entities.Events;
using Domain.Entities.Members;
using Domain.Entities.Posts;
using Domain.Entities.Tags;
using MediatR;

namespace Api.Controllers;

public sealed class EventController : IEndpointDefinition
{
    public void DefineEndpoints(IApplicationBuilder app)
    {
        if (app is not WebApplication)
            throw new ArgumentException("The app needs to be of type WebApplication");

        WebApplication webApp = (WebApplication)app;
        webApp.MapGet("/api/events", GetEventList)
            .WithName("GetEventList")
            .AllowAnonymous()
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest, "application/problem+json");
    }

    public void DefineServices(IServiceCollection services)
    {
        services.AddScoped<IEventRepository, EventRepository>();
    }
    public async Task<IResult> GetEventList(IEventRepository repository, CancellationToken cancellationToken = default)
    {
        var Items = repository.GetAll(cancellationToken);
        return Results.Ok(Items);
    }
}
