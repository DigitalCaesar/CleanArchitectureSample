using Api.Options;
using Api.Startup;
using Data.Startup;
using Infrastructure.BackgroundJobs;

var builder = WebApplication.CreateBuilder(args);

// Build Logging
builder.Logging.AddApplicationLogging();

// Build Options
builder.Services.ConfigureOptions<DatabaseOptionsSetup>();

// Build Services
builder.Services
    .AddProblemDetails()
    .AddDataAccessEntityFramework(builder.Configuration)
    .AddScheduler()
    .AddEmailService()
    .AddMediator()
    .AddValidation()
    .AddExceptionHandling()
    .AddSecurity()
    .AddEndPoints()
    .AddSwagger();

// Build and Run
var app = builder.Build();
app
    .UseExceptionHandling()
    .UseSwaggerCustom()
    .UseEndPoints()
    .UseSecurity();
app.Run();

