namespace EventSquareAPI.DataTypes;

/// <summary>
/// The address component of a result.
/// </summary>
public record GoogleAddressComponent(string long_name, string short_name, string[] types);
