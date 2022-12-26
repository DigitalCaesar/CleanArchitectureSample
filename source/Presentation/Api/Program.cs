using Api.Controllers;
using Api.Middleware;
using Api.Options;
using Api.Startup;
using Application.Behaviors;
using Data.Startup;
using DigitalCaesar.Server.Api;
using FluentValidation;
using Infrastructure.Authentication;
using Infrastructure.BackgroundJobs;
using Infrastructure.Idempotent;
using Infrastructure.Messaging;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//TODO: Move startups to related layers and work up
// Logging
builder.Services.AddProblemDetails();
builder.Logging.AddApplicationLogging();
// Exception handling
builder.Services.AddTransient<ExceptionHandlingMiddleware>();
// Caching
builder.Services.AddMemoryCache();
// Feature Services
builder.Services.AddScoped<IEmailService, EmailService>();
// MediatR
//builder.Services.AddMediator();
builder.Services.AddMediatR(Application.AssemblyReference.Assembly);
builder.Services.AddMediatR(Api.AssemblyReference.Assembly);
builder.Services.Decorate(typeof(INotificationHandler<>), typeof(IdempotentDomainEventHandler<>));
// Validation
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddValidatorsFromAssembly(Application.AssemblyReference.Assembly, includeInternalTypes: true);
// Database
builder.Services.ConfigureOptions<DatabaseOptionsSetup>();
builder.Services.AddDataAccessEntityFramework(builder.Configuration);// DataAccessStrategy.UnitOfWork, CachingInitializationStrategy.Concrete);
// Scheduler
builder.Services.SetupQuartz();
// Security
builder.Services.AddScoped<IJwtProvider, JwtProvider>();
builder.Services.ConfigureOptions<JwtOptionsSetup>();
builder.Services.ConfigureOptions<JwtBearerOptionsSetup>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer();
//    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));
builder.Services.AddAuthorization();
builder.Services.AddAuthentication();
builder.Services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();
// EndPoint Registration
builder.Services.AddEndpointDefinitions(typeof(MemberController));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
                    
                    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Description = "JWT Authorization Header using the bearer token",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey
                    }));
builder.Services.AddSwaggerGen(options => options.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Id = "Bearer",
                                    Type = ReferenceType.SecurityScheme
                                }
                            }, new List<string>()
                        }
                    }));

var app = builder.Build();

//app.UseMiddleware<ExceptionHandlingMiddlewareSimple>();
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseEndpointDefinitions();
app.UseHttpsRedirection();
// Use Security
app.UseAuthentication();
app.UseAuthorization();
//var scopeRequiredByApi = app.Configuration["AzureAd:Scopes"] ?? "";




app.Run();

