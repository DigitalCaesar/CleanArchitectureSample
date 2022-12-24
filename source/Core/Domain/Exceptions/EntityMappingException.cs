namespace Domain.Exceptions;
public class EntityMappingException : DomainException
{
    public EntityMappingException(string entityName) 
        : base($"The {entityName} is in an invalid state.") { }
}
