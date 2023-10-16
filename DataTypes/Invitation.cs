using System.Text.Json.Serialization;

namespace EventSquareAPI.DataTypes;

/// <summary>
/// An event invitation.
/// </summary>
public class Invitation
{
    // Required for migrations, kept private to avoid actual usage.
    /// <summary>
    /// Private constructor for DB migrations.
    /// </summary>
#pragma warning disable CS8618
    private Invitation() { }
#pragma warning restore CS8618 

    /// <summary>
    /// Constructs an invitation.
    /// </summary>
    /// <param name="id">The unique identifier of the invitation.</param>
    /// <param name="receipientId">The unique identifier of the receipient.</param>
    /// <param name="senderId">The unique identifier of the sender.</param>
    /// <param name="eventId">The unique identifier of the event.</param>
    public Invitation(string id, string receipientId, string senderId, string eventId)
    {
        this.Id = id;
        this.ReceipientId = receipientId;
        this.SenderId = senderId;
        this.EventId = eventId;
    }

    /// <summary>
    /// Constructs an invitation.
    /// </summary>
    /// <param name="receipientId">The unique identifier of the receipient.</param>
    /// <param name="senderId">The unique identifier of the sender.</param>
    /// <param name="eventId">The unique identifier of the event.</param>
    [JsonConstructor]
    public Invitation(string receipientId, string senderId, string eventId)
        : this(
              Guid.NewGuid().ToString(),
              receipientId,
              senderId,
              eventId)
    { }

    /// <summary>
    /// The unique identifier of the invitation.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// The unique identifier of the receipient.
    /// </summary>
    public string ReceipientId { get; set; }

    /// <summary>
    /// The unique identifier of the sender.
    /// </summary>
    public string SenderId { get; set; }

    /// <summary>
    /// The unique identifier of the event.
    /// </summary>
    public string EventId { get; set; }
}
