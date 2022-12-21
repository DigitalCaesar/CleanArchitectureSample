using Domain.Entities.Events;
using Domain.Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models;

[Table("DomainEvents")]
public sealed class DomainEventData : IDomainEventData, IDomainEvent
{
    public Guid Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime OccurredOnUtc { get; set; }
    public DateTime? ProcessedOnUtc { get; set; }

    public string? Error { get; set; }
}
