using Domain.Shared;
using FluentValidation;
using MediatR;

namespace Application.Behaviors;
public sealed class ValidationBehavior<TRequest, TResponse> 
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result
{
    private readonly IEnumerable<IValidator<TRequest>> mValidators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        mValidators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken = default)
    {
        // SKip if nothing to validate
        if(!mValidators.Any()) 
            return await next();

        // Validate the request
        var context = new ValidationContext<TRequest>(request);

        // Check for errors
        //var errors = GetErrorsAsExceptions(request);
        Error[] errors = GetErrorsAsArray(request);

        // If there are errors, return them
        if (errors.Any())
            return CreateValidationResult<TResponse>(errors);
            //throw new ValidationException("Validation Failed.");
            //throw new ValidationException(errorsDictionary);
            //TODO: Fix this

        return await next();
    }
    private static TResult CreateValidationResult<TResult>(Error[] errors)
        where TResult : Result
    {
        if(typeof(TResult) == typeof(Result))
            return (ValidationResult.WithErrors(errors) as TResult);

        object? validationResult = typeof(ValidationResult<>)
            .GetGenericTypeDefinition()
            .MakeGenericType(typeof(TResult).GenericTypeArguments)
            .GetMethod(nameof(ValidationResult.WithErrors))!
            .Invoke(null, new object[] { errors });

        return (TResult)validationResult;
    }
    private Error[] GetErrorsAsArray(TRequest request)
    {
        return mValidators
            .Select(validator =>validator.Validate(request))
            .SelectMany(validationResult => validationResult.Errors)
            .Where(validationFailure => validationFailure is not null)
            .Select(failure => new Error(
                failure.PropertyName, 
                failure.ErrorMessage))
            .Distinct()
            .ToArray();
    }
    private Dictionary<string, string[]>? GetErrorsAsExceptions(TRequest request)
    {
        // Validate the request
        var context = new ValidationContext<TRequest>(request);

        var errorsDictionary = mValidators
            .Select(x => x.Validate(context))
            .SelectMany(x => x.Errors)
            .Where(x => x != null)
            .GroupBy(
                x => x.PropertyName,
                x => x.ErrorMessage,
                (propertyName, errorMessages) => new
                {
                    Key = propertyName,
                    Values = errorMessages.Distinct().ToArray()
                })
            .ToDictionary(x => x.Key, x => x.Values);
        return errorsDictionary;
    }
}
