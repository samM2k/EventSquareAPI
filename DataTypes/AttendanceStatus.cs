namespace EventSquareAPI.DataTypes;

/// <summary>
/// The status of whether or not one will attend an event.
/// </summary>
public enum AttendanceStatus
{
    /// <summary>
    /// Maybe.
    /// </summary>
    Maybe,

    /// <summary>
    /// Going.
    /// </summary>
    Going,

    /// <summary>
    /// Attending Virtually.
    /// </summary>
    AttendingVirtually,

    /// <summary>
    /// Not Going.
    /// </summary>
    NotGoing,
}
