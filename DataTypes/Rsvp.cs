using Microsoft.AspNetCore.Identity;

namespace EventSquareAPI.DataTypes;

/// <summary>
/// A given user's RSVP to a given event.
/// </summary>
public class Rsvp
{
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
