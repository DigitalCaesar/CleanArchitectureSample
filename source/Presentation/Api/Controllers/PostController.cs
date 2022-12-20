﻿using Application.Members.Commands.Create;
using Application.Posts.Commands.CreatePost;
using Data.Repositories;
using DigitalCaesar.Server.Api;
using Domain.Entities.Members;
using Domain.Entities.Posts;
using Domain.Entities.Roles;
using Domain.Entities.Tags;
using Domain.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public sealed class PostController : IEndpointDefinition
{
    public void DefineEndpoints(IApplicationBuilder app)
    {
        if (app is not WebApplication)
            throw new ArgumentException("The app needs to be of type WebApplication");

        WebApplication webApp = (WebApplication)app;
        webApp.MapPost("/api/posts", CreatePost)
            .WithName("CreatePost")
            .AllowAnonymous()
            .Produces(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest, "application/problem+json");
        webApp.MapGet("/api/posts", GetPostList)
            .WithName("GetPostList")
            .AllowAnonymous()
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest, "application/problem+json");
    }

    public void DefineServices(IServiceCollection services)
    {
        services.AddScoped<IPostRepository, PostRepository>();
    }
    public async Task<IResult> CreatePost(ISender sender, string name, string content, CancellationToken cancellationToken = default)
    {
        Member author = Member.Create(UserName.Create("TestAuthor").Value, Email.Create("testauthor@someplace.com").Value, FirstName.Create("Test").Value, LastName.Create("Author").Value, new List<Role> { Role.Create(Name.Create("Author").Value, Description.Create("Author Role").Value) });
        List<Tag> tags = new List<Tag>();
        var command = new CreatePostCommand(name, content, author, tags);

        var result = await sender.Send(command, cancellationToken);

        //TODO: Convert to Results.Created - requires URL and Created object
        return result.Successful ? Results.Ok() : Results.BadRequest(result.Error);
    }
    public async Task<IResult> GetPostList(IPostRepository repository, CancellationToken cancellationToken = default)
    {
        var Items = repository.GetAll(cancellationToken);
        return Results.Ok(Items);
    }
}
