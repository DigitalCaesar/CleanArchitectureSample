using Api.Controllers;
using Application.Behaviors;
using Application.Members.Commands.Create;
using Data;
using Data.Repositories;
using DigitalCaesar.Server.Api;
using Domain;
using Domain.Entities.Events;
using Domain.Entities.Members;
using Domain.Entities.Posts;
using Domain.Entities.Roles;
using Domain.ValueObjects;
using FluentValidation.AspNetCore;
using Infrastructure.BackgroundJobs;
using MediatR;
using Messaging;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.Resource;
using System.Threading;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//TODO: Move startups to related layers and work up
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<IMemberRepository, MemberRepository>();
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddMediatR(Application.AssemblyReference.Assembly);
builder.Services.AddMediatR(Api.AssemblyReference.Assembly);
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddFluentValidationAutoValidation();//.AddValidatorsFromAssembly(applicationAssembly);
builder.Services.SetupData(builder.Configuration);
builder.Services.SetupQuartz();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));
builder.Services.AddAuthorization();
builder.Services.AddEndpointDefinitions(typeof(MemberController));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseEndpointDefinitions();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

var scopeRequiredByApi = app.Configuration["AzureAd:Scopes"] ?? "";




app.Run();

