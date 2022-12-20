using Application.Members.Commands.Create;
using Application.Members.Queries;
using Application.Members.Queries.GetMemberById;
using Data.Repositories;
using DigitalCaesar.Server.Api;
using Domain.Entities.Members;
using Domain.Entities.Roles;
using Domain.Shared;
using Domain.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class MemberController : ApiController, IEndpointDefinition
{
    
    public void DefineEndpoints(IApplicationBuilder app)
    {
        if (app is not WebApplication)
            throw new ArgumentException("The app needs to be of type WebApplication");

        WebApplication webApp = (WebApplication)app;
        webApp.MapPost("/api/members", RegisterMember)
            .WithName("RegisterMember")
            .AllowAnonymous()
            .Produces(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest, "application/problem+json");
        webApp.MapGet("/api/members/{id}", GetMemberById)
            .WithName("GetMemberById")
            .AllowAnonymous()
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest, "application/problem+json");
        webApp.MapGet("/api/members", GetMemberList)
            .WithName("GetMemberList")
            .AllowAnonymous()
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest, "application/problem+json");
    }

    public void DefineServices(IServiceCollection services)
    {
        services.AddScoped<IMemberRepository, MemberRepository>();
    }

    public async Task<IResult> RegisterMember(ISender sender, string username, string email, string firstName, string lastName, CancellationToken cancellationToken = default)
    {
        var command = new CreateMemberCommand(
            username,
            email,
            firstName,
            lastName,
            new List<Role>() { Role.Create(Name.Create("Admin").Value, Description.Create("Administrative Users").Value) });

        Result<Guid> result = await sender.Send(command, cancellationToken);

        if(!result.Successful)
            return HandleFailure(result);

        return Results.Created(
            $"/api/members/{result.Value}",
            result.Value);
    }
    public async Task<IResult> GetMemberList(IMemberRepository repository, CancellationToken cancellationToken = default)
    {
        var Items = repository.GetAll(cancellationToken);
        return Results.Ok(Items);
    }
    public async Task<IResult> GetMemberById(ISender sender, Guid id, CancellationToken cancellationToken = default)
    {
        var query = new GetMemberByIdQuery(id);

        Result<MemberResponse> response = await sender.Send(query, cancellationToken);

        return response.Successful ? Results.Ok(response.Value) : Results.NotFound(response.Error);
    }
}
