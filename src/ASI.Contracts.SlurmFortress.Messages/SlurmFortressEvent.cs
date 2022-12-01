using ASI.Services.Messaging;

namespace ASI.Contracts.SlurmFortress.Messages;

/// <summary>
/// Details of an event occurring to be consumed by SlurmFortress
/// </summary>
public class SlurmFortressEvent : IEvent
{
    public string Event { get; set; } = default!;
    public MessageHeader Header { get; set; } = default!;
    public Configuration Configuration { get; set; } = default!;

    public long TenantId { get; set; }
    public long UserId { get; set; }
    public long EntityId { get; set; }

    public const string Added = "myentity.added";
    public const string Updated = "myentity.updated";
    public const string Removed = "myentity.removed";
}