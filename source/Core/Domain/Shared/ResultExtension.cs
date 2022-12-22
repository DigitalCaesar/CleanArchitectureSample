using System.Runtime;

namespace Domain.Shared;
public static class ResultExtension
{
    public static Result<T> Ensure<T>(this Result<T> result, Func<T, bool> predicate, Error error)
    {
        if (!result.Successful)
            return result;
        
        return ((result.Value is not null) && predicate(result.Value))
            ? result
            : Result.Failure<T>(error);
    }
    public static Result<TOut> Map<TIn, TOut>(
        this Result<TIn> result, 
        Func<TIn, TOut> mappingFunc)
    {
        return ((result.Value is not null) && (result.Successful))
            ? Result.Success(mappingFunc(result.Value))
            : Result.Failure<TOut>(result.Error);
    }
}
