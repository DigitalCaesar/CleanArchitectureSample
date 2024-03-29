﻿using Application.Posts.Commands.CreatePost;
using Application.Posts.Queries;
using Application.Posts.Queries.GetPostById;
using Data.Repositories;
using DigitalCaesar.Server.Api;
using Domain.Entities.Posts;
using Domain.Enums;
using Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public sealed class PostController : ApiControllerBase, IEndpointDefinition
{
    public void DefineEndpoints(IApplicationBuilder app)
    {
        if (app is not WebApplication)
            throw new ArgumentException("The app needs to be of type WebApplication");

        WebApplication webApp = (WebApplication)app;
        webApp.MapPost("/api/posts", CreatePost)
            .WithName("CreatePost")
            .RequireAuthorization(Permission.WriteMember.ToString())
            .AllowAnonymous()
            .Produces(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest, "application/problem+json");
        webApp.MapGet("/api/posts/{id}", GetPostById)
            .WithName("GetPostById")
            .RequireAuthorization(Permission.ReadMember.ToString())
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest, "application/problem+json");
        webApp.MapGet("/api/posts", GetPostList)
            .WithName("GetPostList")
            .RequireAuthorization(Permission.ReadMember.ToString())
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest, "application/problem+json");
    }

    public void DefineServices(IServiceCollection services)
    {
        //services.AddScoped<IPostRepository, PostRepository>();
    }
    //public async Task<IResult> CreatePost(ISender sender, string name, string content, CancellationToken cancellationToken = default)
    public async Task<IResult> CreatePost(ISender sender, [FromBody]CreatePostCommand command, CancellationToken cancellationToken = default)
    {
        // TODO: Need to add authentication to pickup on author (logged in member) to set the author property
        //Member author = Member.Create(UserName.Create("TestAuthor").Value, Email.Create("testauthor@someplace.com").Value, FirstName.Create("Test").Value, LastName.Create("Author").Value, new List<Role> { Role.Create(Name.Create("Author").Value, Description.Create("Author Role").Value) });
        //List<string> tags = new List<string>();
        //var command = new CreatePostCommand(name, content, author, tags);

        Result<Guid> result = await sender.Send(command, cancellationToken);

        if (!result.Successful)
            return HandleFailure(result);

        return Results.Created(
            $"/api/posts/{result.Value}",
            result.Value);
    }
    public async Task<IResult> GetPostList(IPostRepository repository, CancellationToken cancellationToken = default)
    {
        //TODO: CONVERT TO CQRS
        var Items = await repository.GetAll(cancellationToken);
        return Results.Ok(Items);
    }
    public async Task<IResult> GetPostById(ISender sender, Guid id, CancellationToken cancellationToken = default)
    {
        var query = new GetPostByIdQuery(id);

        Result<PostResponse> response = await sender.Send(query, cancellationToken);

        return response.Successful ? Results.Ok(response.Value) : Results.NotFound(response.Error);
    }
}
