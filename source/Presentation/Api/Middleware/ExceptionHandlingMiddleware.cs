using Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace Api.Middleware;

internal sealed class ExceptionHandlingMiddleware : IMiddleware
{
    private readonly ILogger<ExceptionHandlingMiddleware> mLogger;
    public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger)
    {
        mLogger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            mLogger.LogError(ex, ex.Message);

            //await HandleExceptionAsync(context, ex);
            await HandleProblem(context, ex);
        }
    }
    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = exception switch
        {
            BadRequestException or ValidationException => StatusCodes.Status400BadRequest,
            NotFoundException => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError
        };
    }
    private static async Task HandleProblem(HttpContext context, Exception exception)
    {
        int StatusCode = (int)HttpStatusCode.InternalServerError;
        context.Response.StatusCode = StatusCode;
        ProblemDetails problem = new()
        {
            Status = StatusCode,
            Type = "Server error",
            Title = "Server Error",
            Detail = "An internal server error has occurred"
        };
        //TODO: Consider more specific types but be careful not to reveal too much
        string json = JsonSerializer.Serialize(problem);
        context.Response.ContentType = "application/problem+json";
        await context.Response.WriteAsync(json);
    }
}
