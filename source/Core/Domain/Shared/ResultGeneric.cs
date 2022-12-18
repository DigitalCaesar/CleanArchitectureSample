using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Shared;
public class Result<T> : Result
{
    private readonly T? mValue;

    protected internal Result(T? value, bool success, Error error)
        : base(success, error)
    {
        mValue = value;
    }

    public T? Value => Successful
        ? mValue!
        : throw new InvalidOperationException("The value of a failure result cannot be accessed.");

    public static implicit operator Result<T>(T value) => Create(value);

    public static Result<T> Create(T value)
    {
        return new(value, true, Error.None);
    }
}
