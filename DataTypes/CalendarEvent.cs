namespace EventSquareAPI.DataTypes;

/// <summary>
/// The event data object.
/// </summary>
/// <remarks>Couldn't just call event due to conflicting type names.</remarks>
public class CalendarEvent
{


    public CalendarEvent(string id, DateTimeOffset startDateTime, DateTimeOffset endDateTime, string name, string description)
    {
        Id = id;
        StartDateTime = startDateTime;
        EndDateTime = endDateTime;
        Name = name;
        Description = description;
    }


    public CalendarEvent(DateTimeOffset startDateTime, DateTimeOffset endDateTime, string name, string description) : this(Guid.NewGuid().ToString(), startDateTime, endDateTime, name, description) { }

    /// <summary>
    /// Gets the unique identifier of the event.
    /// </summary>
    public string Id { get; init; }

    /// <summary>
    /// Gets or sets the start of the event.
    /// </summary>
    public DateTimeOffset StartDateTime { get; set; }
    
    /// <summary>
    /// Gets or sets the end of the event.
    /// </summary>
    public DateTimeOffset EndDateTime { get; set;}

    /// <summary>
    /// Gets or sets the event name/title.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the event description.
    /// </summary>
    public string Description { get; set; }
}
