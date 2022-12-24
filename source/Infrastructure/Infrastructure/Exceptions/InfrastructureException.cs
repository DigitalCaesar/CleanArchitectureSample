namespace Infrastructure.Exceptions;
/// <summary>
/// An exception that occurred in the Infrastructure Layer
/// </summary>
public abstract class DataException : Exception
{
    /// <summary>
    /// Constructor requiring an error message
    /// </summary>
    /// <param name="message">a message describing the exception</param>
    protected DataException(string message) : base(message) { }
}
