namespace Domain.Shared;
public class Result
{
    public bool Successful { get; set; }
    public Error Error { get; set; }

    protected internal Result(bool successful, Error error)
    {
        if (successful && error != Error.None)
            throw new InvalidOperationException();

        if(!successful && error == Error.None)
            throw new InvalidOperationException();

        Successful = successful;
        Error = error;
    }

    public static Result Success() => new(true, Error.None);
    public static Result<T> Success<T>(T value) => new(true, Error.None, value);
    public static Result Failure(Error error) => new(false, error);
    public static Result<T> Failure<T>(Error error) => new(false, error);
    //public static Result Create(bool success, Error error)
    //{
    //    return new(success, error);
    //}

    public static Result<T> Create<T>(T value)
    {
        return new(true, Error.None, value);
    }
}
