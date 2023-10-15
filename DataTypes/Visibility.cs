namespace EventSquareAPI.DataTypes;

/// <summary>
/// An enum for the visibility of an event.
/// </summary>
public enum EventVisibility
{
    /// <summary>
    /// Only the owner can see, no invites can be sent.
    /// </summary>
    Hidden,

    /// <summary>
    /// Only the users invited through the platform can see the event.
    /// </summary>
    InviteOnly,

    /// <summary>
    /// Anyone can see the event.
    /// </summary>
    Public,
}