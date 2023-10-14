using Microsoft.AspNetCore.Identity;

namespace EventSquareAPI.DataTypes;

/// <summary>
/// A given user's RSVP to a given event.
/// </summary>
public class Rsvp
{
    /// <summary>
    /// Constructs an RSVP.
    /// </summary>
    /// <param name="id">The unique identifier of this RSVP.</param>
    /// <param name="eventId">The unique identifier of the event being responded to.</param>
    /// <param name="userId">The unique identifier of the user responding.</param>
    /// <param name="status">The expected attendance status.</param>
    public Rsvp(string id, string eventId, string userId, AttendanceStatus status)
    {
        Id = id;
        EventId = eventId;
        UserId = userId;
        Status = status;
    }

    public Rsvp(string eventId, string userId, AttendanceStatus status): this(Guid.NewGuid().ToString(), eventId, userId, status) { }


    /// <summary>
    /// Gets the unique identifier for the rsvp.
    /// </summary>
    public string Id { get; init; }

    /// <summary>
    /// Gets the Id of the event being responsed to.
    /// </summary>
    public string EventId { get; init; }

    /// <summary>
    /// Gets the Id of the user responding to the event.
    /// </summary>
    public string UserId { get; init; }

    /// <summary>
    /// Gets or sets the user's attendance status (Going, Not Going, etc.).
    /// </summary>
    public AttendanceStatus Status { get; set; }
}
