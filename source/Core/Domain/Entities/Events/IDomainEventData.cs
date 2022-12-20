namespace Domain.Entities.Events;

public interface IDomainEventData
{
    string Content { get; set; }
    string? Error { get; set; }
    Guid Id { get; set; }
    DateTime OccurredOnUtc { get; set; }
    DateTime? ProcessedOnUtc { get; set; }
    string Type { get; set; }
}