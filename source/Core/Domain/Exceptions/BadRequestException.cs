namespace Domain.Exceptions;

public class BadRequestException : DomainException
{
    public BadRequestException(string message) : base(message) { }
}