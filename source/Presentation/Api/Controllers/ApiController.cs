using Domain.Shared;
using FluentValidation.Internal;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;
public abstract class ApiController //: ControllerBase
{
    //protected readonly ISender Sender;

    //protected ApiController(ISender sender)
    //{
    //    Sender = sender;
    //}
    protected IResult HandleFailure(Result result)
    {
        return result switch
        {
            { Successful: true } => throw new InvalidOperationException(),
            IValidationResult validationResult => Results.BadRequest(
                    CreateProblemDetails(
                        "Validation Error", 
                        StatusCodes.Status400BadRequest, 
                        result.Error,
                        validationResult.Errors)), 
            _ => Results.BadRequest(CreateProblemDetails(
                "Bad Request", 
                StatusCodes.Status400BadRequest, 
                result.Error))
        };
    }

    public static ProblemDetails CreateProblemDetails(
        string title, 
        int status, 
        Error error,
        Error[]? errors =null)
    {
        return new ProblemDetails()
        {
            Title = title,
            Type = error.Code,
            Detail = error.Message,
            Status = status,
            Extensions = { { nameof(errors), errors } }
        };
    }
}
