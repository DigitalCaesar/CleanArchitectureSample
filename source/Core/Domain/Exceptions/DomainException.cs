namespace Domain.Exceptions;
/// <summary>
/// An exception that occurred in the Domain Layer
/// </summary>
public abstract class DomainException : Exception
{
    /// <summary>
    /// Constructor requiring a error message
    /// </summary>
    /// <param name="message">a message describing the exception</param>
    protected DomainException(string message) : base(message) { }
}
