using System.Text.Json.Serialization;

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
    /// <param name="calendarEvent">The event being responded to..</param>
    public Rsvp(string id, string eventId, string userId, AttendanceStatus status, CalendarEvent? calendarEvent)
    {
        Id = id;
        EventId = eventId;
        UserId = userId;
        Status = status;
        Event = calendarEvent;
    }

    // This is the one we use for EF Core as it doesn't require Id pr linked Event object but can process them post-construction as required.
    /// <summary>
    /// An RSVP or response to an event.
    /// </summary>
    /// <param name="eventId">The unique identifier of the event.</param>
    /// <param name="userId">The unique identifier of the user responding.</param>
    /// <param name="status">Whether this user will attend this event and how.</param>
    [JsonConstructor]
    public Rsvp(string eventId, string userId, AttendanceStatus status) : this(Guid.NewGuid().ToString(), eventId, userId, status, null) { }


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

    /// <summary>
    /// Gets the linked event.
    /// </summary>
    /// <remarks>Used for EFCore linking.</remarks>
    public CalendarEvent? Event { get; init; }
}
