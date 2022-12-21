
namespace Domain.Shared;
public interface IAuditableEntity
{
    DateTime CreatedOnUtc { get; set; }
    DateTime ModifiedOnUtc { get; set; }
}
