namespace Application.Exceptions;

/// <summary>
/// An exception that occurred in the application layer
/// </summary>
public abstract class ApplicationException : Exception
{
    /// <summary>
    /// Constructor requiring an error message
    /// </summary>
    /// <param name="message">a message describing the exception</param>
    protected ApplicationException(string message) : base(message) { }
}
