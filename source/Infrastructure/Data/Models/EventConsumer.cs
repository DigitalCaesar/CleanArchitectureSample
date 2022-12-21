

namespace Data.Models;

/// <summary>
/// Registers the execution of event handlers
/// </summary>
public sealed class EventConsumer
{
    /// <summary>
    /// The Id of the Event
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// The name of the event consumer
    /// </summary>
    public string Name { get; set; } = string.Empty;
}
