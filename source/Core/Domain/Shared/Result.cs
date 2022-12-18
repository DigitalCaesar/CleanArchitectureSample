using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    //public static Result<TValue> Success(TValue value) => new(value, Error.None);
    public static Result Failure(Error error) => new(false, error);
    public static Result Create(bool success, Error error)
    {
        return new(success, error);
    }
}
