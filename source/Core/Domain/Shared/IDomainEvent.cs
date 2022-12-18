
using MediatR;

namespace Domain.Shared;

/// <summary>
/// Interface for recording things that have happened in the system
/// </summary>
public interface IDomainEvent : INotification
{
}
