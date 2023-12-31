﻿using System.Text.Json.Serialization;

namespace EventSquareAPI.DataTypes;

/// <summary>
/// The event data object.
/// </summary>
/// <remarks>Couldn't just call event due to conflicting type names.</remarks>
public class CalendarEvent
{
    // Required for migrations, kept private to avoid actual usage.
    /// <summary>
    /// Private constructor for DB migrations.
    /// </summary>
#pragma warning disable CS8618
    private CalendarEvent() { }
#pragma warning restore CS8618 

    /// <summary>
    /// Constructs a calendar event.
    /// </summary>
    /// <param name="id">The Unique Identifier of the event.</param>
    /// <param name="startDateTime">The DateTimeOffset at which the event starts.</param>
    /// <param name="endDateTime">The DateTimeOffset at which the event ends.</param>
    /// <param name="name">The name of the event.</param>
    /// <param name="description">The event description.</param>
    /// <param name="isVirtual">Whether or not the event can be attended virtually.</param>
    /// <param name="isPhysical">Whether or not the event can be attended in person.</param>
    /// <param name="owner">The owner of the event..</param>
    /// <param name="visibility">The visibility of the event.</param>
    /// <param name="rsvps">The RSVPs to the event.</param>
    /// <param name="location">The location of the event</param>
    public CalendarEvent(string id, DateTimeOffset startDateTime, DateTimeOffset endDateTime, string name, string description, bool isVirtual, bool isPhysical, Location? location, string owner, EventVisibility visibility, List<Rsvp> rsvps)
    {
        this.Id = id;
        this.StartDateTime = startDateTime;
        this.EndDateTime = endDateTime;
        this.Name = name;
        this.Description = description;
        this.IsVirtual = isVirtual;
        this.IsPhysical = isPhysical;
        this.Location = location;
        this.Rsvps = rsvps;
        this.Owner = owner;
        this.Visibility = visibility;
    }

    // This is the one we use for EF Core as it doesn't require ID or RSVPs but can attach them post-construction if provided.
    /// <summary>
    /// An event.
    /// </summary>
    /// <param name="startDateTime">The start of the event.</param>
    /// <param name="endDateTime">The end of the event.</param>
    /// <param name="name">The event name.</param>
    /// <param name="description">The event description.</param>
    /// <param name="isVirtual">Whether or not the event can be attended virtually.</param>
    /// <param name="isPhysical">Whether or not the event can be attended in-person.</param>
    /// <param name="owner">The creator of the event.</param>
    /// <param name="location">The location of the event.</param>
    [JsonConstructor]
    public CalendarEvent(DateTimeOffset startDateTime, DateTimeOffset endDateTime, string name, string description, bool isVirtual, bool isPhysical, string owner, Location? location)
        : this(Guid.NewGuid().ToString(), startDateTime, endDateTime, name, description, isVirtual, isPhysical, location, owner, EventVisibility.Hidden, new List<Rsvp>()) { }

    /// <summary>
    /// Gets the unique identifier of the event.
    /// </summary>
    public string Id { get; init; }

    /// <summary>
    /// Gets the ID of the owner of the event.
    /// </summary>
    public string Owner { get; set; }

    /// <summary>
    /// Gets or sets the visibility of the event.
    /// </summary>
    public EventVisibility Visibility { get; set; }

    /// <summary>
    /// Gets or sets the start of the event.
    /// </summary>
    public DateTimeOffset StartDateTime { get; set; }

    /// <summary>
    /// Gets or sets the end of the event.
    /// </summary>
    public DateTimeOffset EndDateTime { get; set; }

    /// <summary>
    /// Gets or sets the event name/title.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the event description.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets whether the event can be attended virtually.
    /// </summary>
    public bool IsVirtual { get; set; }

    /// <summary>
    /// Gets or sets whether the event can be attended in-person.
    /// </summary>
    public bool IsPhysical { get; set; }

    /// <summary>
    /// Gets or sets the location of the event.
    /// </summary>
    public Location? Location { get; set; }

    /// <summary>
    /// Gets the RSVPs for the event. Used for EFCOre linking.
    /// </summary>
    public ICollection<Rsvp> Rsvps { get; init; }
}
