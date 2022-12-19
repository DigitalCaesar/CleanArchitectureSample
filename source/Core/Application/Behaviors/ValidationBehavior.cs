using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Behaviors;
public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class, IRequest<TResponse> //ICommand<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> mValidators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        mValidators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken = default)
    {
        if(!mValidators.Any()) 
            return await next();

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

        if (errorsDictionary.Any())
            throw new ValidationException("Validation Failed.");
            //throw new ValidationException(errorsDictionary);
            //TODO: Fix this

        return await next();
    }
}
